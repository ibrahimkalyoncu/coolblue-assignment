using System;
using System.Linq;
using Insurance.Api.Controllers;
using Insurance.ConnectedServices.ProductApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xunit;

namespace Insurance.Tests
{
    public class InsuranceTests: IClassFixture<ControllerTestFixture>
    {
        [Fact]
        public void CalculateInsurance_GivenSalesPriceBetween500And2000Euros_ShouldAddThousandEurosToInsuranceCost()
        {
            const float expectedInsuranceValue = 1000;

            var dto = new HomeController.InsuranceDto
                      {
                          ProductId = 1,
                      };
            var sut = new HomeController();

            var result = sut.CalculateInsurance(dto);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }
        
        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        public void CalculateInsurance_GivenAProductWithExtraInsuranceAmount_ShouldAdd500EurosToInsuranceCost(int productId)
        {
            const float expectedInsuranceValue = 1500;

            var dto = new HomeController.InsuranceDto { ProductId = productId };
            
            var sut = new HomeController();

            var result = sut.CalculateInsurance(dto);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }
    }

    public class ControllerTestFixture: IDisposable
    {
        private readonly IHost _host;

        public ControllerTestFixture()
        {
            _host = new HostBuilder()
                   .ConfigureWebHostDefaults(
                        b => b.UseUrls("http://localhost:5002")
                              .UseStartup<ControllerTestStartup>()
                    )
                   .Build();

            _host.Start();
        }

        public void Dispose() => _host.Dispose();
    }

    public class ControllerTestStartup
    {
        public void Configure(IApplicationBuilder app)
        {
            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            };
            
            var productsTestData = new[]
            {
                new ProductDto(1,"Test Simple Product", 750, 1), 
                new ProductDto(2,"Test Extra Insured Product", 750, 2), 
                new ProductDto(3,"Test Extra Insured Product", 750, 3), 
            }; 
            
            var productTypesTestData = new[]
            {
                new ProductTypeDto(1,"Test Simple Product Type", true), 
                new ProductTypeDto(2,"Laptops", true),
                new ProductTypeDto(3,"Smartphones", true) 
            }; 
            
            app.UseRouting();
            app.UseEndpoints(
                ep =>
                {
                    ep.MapGet(
                        "products/{id:int}",
                        context =>
                        {
                            int productId = int.Parse((string) context.Request.RouteValues["id"]);
                            return context
                                .Response
                                .WriteAsync(JsonConvert.SerializeObject(productsTestData.FirstOrDefault(x => x.Id == productId), jsonSettings));
                        }
                    );
                    ep.MapGet(
                        "product_types",
                        context => context.Response.WriteAsync(JsonConvert.SerializeObject(productTypesTestData, jsonSettings)));
                }
            );
        }
    }
}