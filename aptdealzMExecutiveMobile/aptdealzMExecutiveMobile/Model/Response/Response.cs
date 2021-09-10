﻿using Newtonsoft.Json;

namespace aptdealzMExecutiveMobile.Model.Response
{
    public class Response
    {
        [JsonProperty("pageNumber")]
        public int PageNumber { get; set; }

        [JsonProperty("pageSize")]
        public int PageSize { get; set; }

        [JsonProperty("succeeded")]
        public bool Succeeded { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("errors")]
        public object Errors { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }
    }
}
