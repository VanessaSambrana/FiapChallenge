using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
using HtmlAgilityPack;

namespace Procurados.Models
{
    public class IdwallConfig
	{
        private HttpClient _httpClient;

        public IdwallConfig()
		{
		}

        public void SetHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddScoped<IdwallConfig>();

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FiapChallenge", Version = "v1" });
            });
        }


        public void ConfigureDevelopment(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ConfigureCommon(app, env);
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ConfigureCommon(app, env);
        }

        private void ConfigureCommon(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "FiapChallenge");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("FiapChallenge API");
                });
            });
        }


        public async Task<bool> ProcuradoFBIByTitle(string title)
        {

            string url = $"https://www.fbi.gov/wanted/topten/{title.ToLower().Replace(" ", "-")}";
            HttpClient httpClient = new HttpClient();

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string html = await response.Content.ReadAsStringAsync();

                    bool isWanted = ParseHTMLForFBIWanted(html, title.ToUpper());
                    return isWanted;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool ParseHTMLForFBIWanted(string html, string title)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            HtmlNode titleNode = htmlDoc.DocumentNode.SelectSingleNode("//title");
            if (titleNode != null && titleNode.InnerText.Contains(title))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public async Task<bool> ProcuradoInterpol(string forename, string name, string? dateOfBirth = null)
        {
            forename = forename.ToUpper();
            name = name.ToUpper();
            string apiUrl = $"https://ws-public.interpol.int/notices/v1/red/";
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                bool isWantedByForename = IsPersonInJson(json, forename, "forename");
                bool isWantedByName = IsPersonInJson(json, name, "name");
                bool isWantedByDateOfBirth = !string.IsNullOrEmpty(dateOfBirth) && IsPersonInJson(json, dateOfBirth, "date_of_birth");



                return isWantedByForename || isWantedByName || isWantedByDateOfBirth;
            }
            return false;
        }

        private bool IsPersonInJson(string noticesJson, string searchTerm, string property)
        {
            JObject jsonObject = JObject.Parse(noticesJson);

            if (jsonObject.TryGetValue("_embedded", out JToken embeddedToken) && embeddedToken.Type == JTokenType.Object)
            {
                JObject embeddedObject = (JObject)embeddedToken;

                if (embeddedObject.TryGetValue("notices", out JToken noticesToken) && noticesToken.Type == JTokenType.Array)
                {
                    JArray noticesArray = (JArray)noticesToken;

                    foreach (JToken noticeToken in noticesArray)
                    {
                        JToken searchToken = noticeToken.SelectToken(property);

                        if (searchToken != null && searchToken.Value<string>() == searchTerm)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}




   