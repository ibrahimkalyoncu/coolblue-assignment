﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Insurance.ConnectedServices.ProductApi
{
    public class ProductApiClient : IProductApiClient
    {
        private readonly HttpClient _client;

        private const string GetProductByIdPath = "/products/{0}";
        private const string GetProductTypeByIdPath = "/product_types/{0}";

        public ProductApiClient(IHttpClientFactory httpClientFactory,
            IProductApiConfiguration configuration)
        {
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri(configuration.BaseAddress);
        }
        
        public async Task<ProductDto> GetProductByIdAsync(int productId)
        {
            var response = await _client.GetAsync(string.Format(GetProductByIdPath, productId));
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<ProductDto>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ProductTypeDto> GetProductTypeByIdAsync(int productTypeId)
        {
            var response = await _client.GetAsync(string.Format(GetProductTypeByIdPath, productTypeId));
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<ProductTypeDto>(await response.Content.ReadAsStringAsync());        }
    }
}