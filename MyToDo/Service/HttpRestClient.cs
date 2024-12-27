﻿using MyToDo.Shared;
using MyToDo.Shared.Contact;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace MyToDo.Service
{
    public class HttpRestClient
    {
        private readonly string apiUrl;
        protected readonly RestClient client;

        public HttpRestClient(string apiUrl)
        {
            this.apiUrl = apiUrl;
            this.client = new RestClient();
        }

        public async Task<ApiResponse> ExecuteAsync(BaseRequest baseRequest)
        {
            var request = new RestRequest(baseRequest.Method);
            request.AddHeader("Content-Type", baseRequest.ContentType);

            if (baseRequest.Parameter != null)
                request.AddParameter("param", JsonConvert.SerializeObject(baseRequest.Parameter), ParameterType.RequestBody);
            client.BaseUrl = new Uri(apiUrl + baseRequest.Route);
            var response = await client.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<ApiResponse>(response.Content);

            else
                return new ApiResponse()
                {
                    Status = false,
                    Result = null,
                    Message = response.ErrorMessage
                };
        }


        public async Task<ApiResponse<T>> ExecuteAsync<T>(BaseRequest baseRequest)
        {


            var request = new RestRequest(baseRequest.Method);
            request.AddHeader("Content-Type", baseRequest.ContentType);

            if (baseRequest.Parameter != null)
                request.AddParameter("param", JsonConvert.SerializeObject(baseRequest.Parameter), ParameterType.RequestBody);

            // 创建 RestClient 实例并设置 BaseUrl
            var client = new RestClient(new Uri(apiUrl + baseRequest.Route));

            var response = await client.ExecuteAsync(request);

            return JsonConvert.DeserializeObject<ApiResponse<T>>(response.Content);


            //var request = new RestRequest(baseRequest.Method);
            //request.AddHeader("Content-Type", baseRequest.ContentType);

            //if (baseRequest.Parameter != null)
            //    request.AddParameter("param", JsonConvert.SerializeObject(baseRequest.Parameter), ParameterType.RequestBody);
            //client.BaseUrl = new Uri(apiUrl + baseRequest.Route);
            //var response = await client.ExecuteAsync(request);

            //return JsonConvert.DeserializeObject<ApiResponse<T>>(response.Content);
        }
    }
}
