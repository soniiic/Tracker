using Microsoft.AspNetCore.Mvc;

namespace Tracker.Web.Controllers
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    [Route("api/[controller]")]
    [ApiController]
    public class TrackController : ControllerBase
    {
        [Route("{hash}")]
        public string Get(string hash)
        {
            var myString = Request.HttpContext.Connection.RemoteIpAddress + "|" + hash;

            var hash1 = GetMD5(myString);
            Guid guid;
            if (!Storage.Dic.TryGetValue(hash1, out guid))
            {
                guid = Guid.NewGuid();
                Storage.Dic.Add(hash1, guid);
            }

            return guid.ToString();
        }

        private static string GetMD5(string myString)
        {
            using (MD5 md5 = MD5.Create())
            {
                return string.Concat(md5.ComputeHash(Encoding.UTF8.GetBytes(myString)).Select(ba => ba.ToString("x2")));
            }
        }
    }

    public static class Storage
    {
        public static IDictionary<string, Guid> Dic = new ConcurrentDictionary<string, Guid>();
    }
}