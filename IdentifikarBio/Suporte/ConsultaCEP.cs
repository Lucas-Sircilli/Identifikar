using IdentifikarBio.Database.Objetos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IdentifikarBio.Suporte
{
    public class ConsultaCEP
    {
        public ObRespCep consultaPorLogradouro(string uf, string cidade, string logradouro, LogFile log)
        {
            try
            {
                WebClient wc = new WebClient();
                wc.Encoding = Encoding.UTF8;
                var url = "https://viacep.com.br/ws/" + Uri.EscapeDataString(uf) + "/" + Uri.EscapeDataString(cidade) + "/" + Uri.EscapeDataString(logradouro) + "/json/";
                log.write("ENDERECO: " + url);
                string result = wc.DownloadString(url);
                var RetCep = JsonConvert.DeserializeObject<List<ObRespCep>>(result);

                if (RetCep.Count > 1)
                {
                    return RetCep[0];
                }
                else if (RetCep.Count == 1)
                    return RetCep[0];
                else return null;
            }
            catch (Exception e)
            {
                log.write("falha consultando cep", e);
                return null;
            }
        }

        public static ObProxy getUserProxy()
        {
            ObProxy obProxy = null;
            try
            {
                if (File.Exists(".\\Proxy.conf"))
                {
                    var jsce = File.ReadAllText(".\\Proxy.conf");
                    if (!String.IsNullOrEmpty(jsce))
                    {
                        var json = Funcoes.Decrypt(jsce, true);
                        obProxy = JsonConvert.DeserializeObject<ObProxy>(json);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return obProxy;
        }

        

        public static ObRespCep consultaPorCep(string cep, LogFile log)
        {
            try
            {
                WebClient wc = new WebClient();

                var ppx = getUserProxy();
                if (ppx != null && !string.IsNullOrEmpty(ppx.userProxy) && !string.IsNullOrEmpty(ppx.passProxy))
                {

                    IWebProxy proxy = wc.Proxy;
                    if (proxy != null)
                    {
                        log.write("PROXY ENCONTRADO");
                        var proxyuri = proxy.GetProxy(new Uri("http://viacep.com.br/ws/" + cep + "/json/")).ToString();
                        WebProxy myProxy = new WebProxy();
                        myProxy.Address = new Uri(proxyuri);
                        myProxy.Credentials = new NetworkCredential(ppx.userProxy, ppx.passProxy);
                        //myProxy.Credentials = new NetworkCredential("treinamento.ds1@grupocriar", "Identificar!1");
                        wc.Proxy = myProxy;
                        log.write("ENDERECO PROXY: " + proxyuri);
                        //request.UseDefaultCredentials = true;
                        /*request.Proxy = new WebProxy(proxyuri, false);
                        request.Proxy.Credentials = new NetworkCredential("treinamento.ds1@grupocriar", "Identificarl1"); ;*/

                        log.write("SUCESSO PROXY: ");
                    }
                    else
                    {
                        log.write("PROXY NÃO ENCONTRADO");
                        wc.Credentials = CredentialCache.DefaultCredentials;
                    }
                }
                else
                {
                    log.write("PROXY NÃO ENCONTRADO");
                    wc.Credentials = CredentialCache.DefaultCredentials;
                }
                wc.Encoding = Encoding.UTF8;
                string result = wc.DownloadString("http://viacep.com.br/ws/" + cep + "/json/");
                var RetCep = JsonConvert.DeserializeObject<ObRespCep>(result);
                if (RetCep.erro == "true")
                {
                    return null;
                }
                RetCep.bairro = RetCep.bairro != null ? RetCep.bairro : "";
                RetCep.logradouro = RetCep.logradouro != null ? RetCep.logradouro : "";
                RetCep.uf = RetCep.uf != null ? RetCep.uf : "";
                
                // RetCep = await _uow.EntidadeRepository.verificaCEP(RetCep);
                return RetCep;
            }
            catch (Exception ex)
            {
                log.write("falha consultando cep",ex);
                return null;
            }
        }
    }

    public class ObRespCep
    {
        public string cep { get; set; }
        public string logradouro { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string localidade { get; set; }
        public string uf { get; set; }
        public string ibge { get; set; }
        public string gia { get; set; }
        public string ddd { get; set; }
        public string siafi { get; set; }

        public string erro { get; set; }
    }

}
