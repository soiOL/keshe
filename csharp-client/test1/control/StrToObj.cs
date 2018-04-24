using System;
using Newtonsoft.Json;
using test1.data;

namespace test1.control
{
    class StrToObj
    {
    }
    

    /// <summary>
    /// JsonToObject 的摘要说明
    /// </summary>
    public class JsonToObject
    {
        private readonly string _json;

        public Type _jsonType { get; set; }

        public object jsonObject { get; set; }

        public JsonToObject(string json)
        {
            _json = json;
            IsBindStruct();
            //IsInfoStruct();
            IsSendDatatStruct();
        }

        private void IsBindStruct()
        {
            if (_jsonType != null)
            {
                return;

            }
            var bindstruct = (BindStruct)JsonConvert.DeserializeObject(_json, typeof(BindStruct));
            if (bindstruct.action == null) return;
            _jsonType = typeof(BindStruct);
            jsonObject = bindstruct;
        }

        private void IsSendDatatStruct()
        {
            if (_jsonType != null)
            {
                return;

            }
            var sendDataStruct = (BaseStruct)JsonConvert.DeserializeObject(_json, typeof(BaseStruct));
            //if (sendDataStruct.LastDataTime == null) return;
            _jsonType = typeof(BaseStruct);
            jsonObject = sendDataStruct;
        }

        //private void IsInfoStruct()
        //{
        //    if (_jsonType != null)
        //    {
        //        return;

        //    }
        //    var infoStruct = (InfoStruct)JsonConvert.DeserializeObject(_json, typeof(InfoStruct));
        //    if (infoStruct.Date == null) return;
        //    _jsonType = typeof(InfoStruct);
        //    jsonObject = infoStruct;
        //}

    }
}
