﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ServiceComposer.AspNetCore;
using JsonUtils;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shipping.ViewModelComposition
{
    class ProductDetailsGetHandler : IHandleRequests
    {
        public bool Matches(RouteData routeData, string httpVerb, HttpRequest request)
        {
            var controller = (string)routeData.Values["controller"];

            return HttpMethods.IsGet(httpVerb)
                   && controller.ToLowerInvariant() == "products"
                   && routeData.Values.ContainsKey("id");
        }

        public async Task Handle(string requestId, dynamic vm, RouteData routeData, HttpRequest request)
        {
            var id = (string)routeData.Values["id"];

            var url = $"http://localhost:5004/api/shipping-options/product/{id}";
            var client = new HttpClient();
            var response = await client.GetAsync(url);

            dynamic productShippingOptions = await response.Content.AsExpando();

            var options = ((IEnumerable<dynamic>)productShippingOptions.Options)
                .Select(o => o.Option)
                .ToArray();

            vm.ProductShippingOptions = string.Join(", ", options);
        }
    }
}
