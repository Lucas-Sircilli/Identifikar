using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using IdentifikarBio.Interface;
using IdentifikarBio.Database.Objetos;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using IdentifikarBio.Suporte;
using System.Security.Cryptography;

namespace IdentifikarBio.Database
{
    class GerenteConexoes
    {
        public string responseFromServer;
        private static ObToken token = new ObToken();
        private static ObRespToken resptoken = new ObRespToken();

        public static string proxyuri = "";
        

        public ObRespToken AcessaWebservice(ObToken usuario)
        {
            var log = new LogFile(Parametros.EnderecoLog);
            token = usuario;
            WebRequest request = WebRequest.Create("https://api.identifikar.com.br/v1/rest/token");
            request.Method = "POST";
            request.Timeout = 20000;

            log.write("VERIFICANDO PROXY DE ENTRADA");

            var ppx = Funcoes.getUserProxy();
            if (ppx != null && !string.IsNullOrEmpty(ppx.userProxy) && !string.IsNullOrEmpty(ppx.passProxy))
            {

                IWebProxy proxy = request.Proxy;
                if (proxy != null)
                {
                    log.write("PROXY ENCONTRADO");
                    proxyuri = proxy.GetProxy(request.RequestUri).ToString();
                    WebProxy myProxy = new WebProxy();
                    myProxy.Address = new Uri(proxyuri);
                    myProxy.Credentials = new NetworkCredential(ppx.userProxy, ppx.passProxy);
                    //myProxy.Credentials = new NetworkCredential("treinamento.ds1@grupocriar", "Identificar!1");
                    request.Proxy = myProxy;
                    log.write("ENDERECO PROXY: " + proxyuri);
                    //request.UseDefaultCredentials = true;
                    /*request.Proxy = new WebProxy(proxyuri, false);
                    request.Proxy.Credentials = new NetworkCredential("treinamento.ds1@grupocriar", "Identificarl1"); ;*/

                    log.write("SUCESSO PROXY: ");
                }
                else
                {
                    log.write("PROXY NÃO ENCONTRADO");
                    request.Credentials = CredentialCache.DefaultCredentials;
                }
            }
            else
            {
                log.write("PROXY NÃO ENCONTRADO");
                request.Credentials = CredentialCache.DefaultCredentials;
            }

            string envio = JsonConvert.SerializeObject(token);
            var data = Encoding.UTF8.GetBytes(envio);
            request.ContentType = "application/json";

            request.ContentLength = data.Length;

            var newStream = request.GetRequestStream();
            newStream.Write(data, 0, data.Length);

            responseFromServer = "";
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(dataStream);
                        responseFromServer = reader.ReadToEnd();
                        // string[] tokens;
                        // tokens = responseFromServer.Split('"');
                        var obRespToken = JsonConvert.DeserializeObject<ObRespToken>(responseFromServer);
                        resptoken = obRespToken;
                        return obRespToken;
                        /*resptoken.accessToken = tokens[3];
                        resptoken.tokenType = tokens[7];
                        resptoken.expiresIn = int.Parse(tokens[10].Remove(6).Substring(1));*/
                        //resptoken.admin = 
                    }
                }

            }
            catch (WebException ex)
            {
                responseFromServer = "";
                if (ex.Response != null)
                {
                    using (WebResponse response = ex.Response)
                    {
                        Stream dataRs = response.GetResponseStream();
                        using (StreamReader reader = new StreamReader(dataRs))
                        {
                            var msgRet = "";
                            responseFromServer += reader.ReadToEnd();
                            var obRespToken = JsonConvert.DeserializeObject<ObRespToken>(responseFromServer);
                            resptoken = obRespToken;
                            return obRespToken;
                        }
                    }
                }
                return null;
            }
           

        }

        

        
        public void IncluiAlteraDado(ObCadastro cadastro)
        {
            try
            {
                // cadastroWebService = cadastro;
                WebRequest request = WebRequest.Create("https://api.identifikar.com.br/v1/rest/cadastro");
                //request.Credentials = CredentialCache.DefaultCredentials;
                request.Method = "POST"; request.Timeout = 20000;
                request.Headers["Authorization"] = resptoken.token_type + " " + resptoken.access_token + "";

                var ppx = Funcoes.getUserProxy();
                if (ppx != null && !string.IsNullOrEmpty(ppx.userProxy) && !string.IsNullOrEmpty(ppx.passProxy))
                {

                    IWebProxy proxy = request.Proxy;
                    if (proxy != null)
                    {
                        proxyuri = proxy.GetProxy(request.RequestUri).ToString();
                        WebProxy myProxy = new WebProxy();
                        myProxy.Address = new Uri(proxyuri);
                        myProxy.Credentials = new NetworkCredential(ppx.userProxy, ppx.passProxy);
                        //myProxy.Credentials = new NetworkCredential("treinamento.ds1@grupocriar", "Identificar!1");
                        request.Proxy = myProxy;

                    }
                    else
                    {
                        request.Credentials = CredentialCache.DefaultCredentials;
                    }
                }
                else
                {
                    request.Credentials = CredentialCache.DefaultCredentials;
                }

                cadastro.excecao_digital = "0";
                string envio = JsonConvert.SerializeObject(cadastro);

                var data = Encoding.UTF8.GetBytes(envio);
                request.ContentType = "application/json";

                request.ContentLength = data.Length;

                var newStream = request.GetRequestStream();
                newStream.Write(data, 0, data.Length);

                responseFromServer = "";
                try
                {
                    using (WebResponse response = request.GetResponse())
                    {
                        using (Stream dataStream = response.GetResponseStream())
                        {
                            StreamReader reader = new StreamReader(dataStream);
                            responseFromServer = reader.ReadToEnd();
                            /* var obRespCadastro = JsonConvert.DeserializeObject<ObRespCadastro>(responseFromServer);
                             return obRespCadastro;*/
                        }
                    }
                }
                catch (WebException ex)
                {
                    responseFromServer = "";
                    if (ex.Response != null)
                    {
                        using (WebResponse response = ex.Response)
                        {
                            Stream dataRs = response.GetResponseStream();
                            using (StreamReader reader = new StreamReader(dataRs))
                            {
                                var msgRet = "";
                                responseFromServer += reader.ReadToEnd();
                                var obj = JsonConvert.DeserializeObject<ObRetorno>(responseFromServer);
                                throw new Exception(obj.codigoRetorno + " - " + obj.mensagem);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                if (e.Message.Contains("O nome remoto não pôde ser resolvido"))
                {
                    throw new Exception("Não foi realizar conexão com o servidor\r\nVerifique a sua conexão e tente novamente");
                }
                else
                {
                    throw new Exception("Falha na tentativa de conexão ao servidor\r\n" + e.Message);
                }
            }
        }

        public ObRespCadastro ConsultaDado(ObCadastro cadastro, int dado)
        {
            try
            {
                // Variável é o tipo de operação que vai ser feita no método Consulta Dado, que é inserida na classe especificada:
                //dado = 1 => Cpf
                //dado = 2 => id candidato
                //dado = 3 => renach
                string parametros = "";
                if (dado == 1)
                    parametros = "?cpf=" + cadastro.cpf;
                else if (dado == 2)
                    parametros = "?id_candidatos=" + cadastro.id_candidatos;
                else if (dado == 3)
                    parametros = "?renach=" + cadastro.renach;

                WebRequest request = WebRequest.Create("https://api.identifikar.com.br/v1/rest/cadastro" + parametros);
                request.Timeout = 20000;
                //request.Credentials = CredentialCache.DefaultCredentials;
                request.Method = "GET";
                request.Headers["Authorization"] = resptoken.token_type + " " + resptoken.access_token + "";
                string envio = JsonConvert.SerializeObject(cadastro);

                var ppx = Funcoes.getUserProxy();
                if (ppx != null && !string.IsNullOrEmpty(ppx.userProxy) && !string.IsNullOrEmpty(ppx.passProxy))
                {

                    IWebProxy proxy = request.Proxy;
                    if (proxy != null)
                    {
                        proxyuri = proxy.GetProxy(request.RequestUri).ToString();
                        WebProxy myProxy = new WebProxy();
                        myProxy.Address = new Uri(proxyuri);
                        myProxy.Credentials = new NetworkCredential(ppx.userProxy, ppx.passProxy);
                        //myProxy.Credentials = new NetworkCredential("treinamento.ds1@grupocriar", "Identificar!1");
                        request.Proxy = myProxy;

                    }
                    else
                    {
                        request.Credentials = CredentialCache.DefaultCredentials;
                    }
                }
                else
                {
                    request.Credentials = CredentialCache.DefaultCredentials;
                }

                var data = Encoding.UTF8.GetBytes(envio);
                request.ContentType = "application/json";
                responseFromServer = "";
                try
                {
                    using (WebResponse response = request.GetResponse())
                    {
                        using (Stream dataStream = response.GetResponseStream())
                        {
                            StreamReader reader = new StreamReader(dataStream);
                            responseFromServer = reader.ReadToEnd();
                            responseFromServer = responseFromServer.Replace("\"registro_expedicao\":\"0000-00-00\",", null);
                            responseFromServer = responseFromServer.Replace("\"nascimento\":\"0000-00-00\",", null);
                            var obRespCadastro = JsonConvert.DeserializeObject<ObRespCadastro>(responseFromServer);
                            return obRespCadastro;
                        }
                    }
                }
                catch (WebException ex)
                {
                    responseFromServer = "";
                    if (ex.Response != null)
                    {
                        using (WebResponse response = ex.Response)
                        {
                            Stream dataRs = response.GetResponseStream();
                            using (StreamReader reader = new StreamReader(dataRs))
                            {
                                var msgRet = "";
                                responseFromServer += reader.ReadToEnd();
                                var obj = JsonConvert.DeserializeObject<ObRetorno>(responseFromServer);
                                throw new Exception(obj.codigoRetorno + " - " + obj.mensagem);
                            }
                        }
                    }
                    return null;
                }
            }
            catch (Exception e)
            {
                if (e.Message.Contains("O nome remoto não pôde ser resolvido"))
                {
                    throw new Exception("Não foi realizar conexão com o servidor\r\nVerifique a sua conexão e tente novamente");
                }
                else
                {
                    throw new Exception("Falha na tentativa de conexão ao servidor\r\n" + e.Message);
                }
            }
        }

        public ObRespMunicipio listaMunicipio(string uf)
        {
            try
            {
                WebRequest request = WebRequest.Create("https://api.identifikar.com.br/v1/rest/municipios/" + uf);
                request.Timeout = 20000;
                //request.Credentials = CredentialCache.DefaultCredentials;
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Headers["Authorization"] = resptoken.token_type + " " + resptoken.access_token + "";

                var ppx = Funcoes.getUserProxy();
                if (ppx != null && !string.IsNullOrEmpty(ppx.userProxy) && !string.IsNullOrEmpty(ppx.passProxy))
                {

                    IWebProxy proxy = request.Proxy;
                    if (proxy != null)
                    {
                        proxyuri = proxy.GetProxy(request.RequestUri).ToString();
                        WebProxy myProxy = new WebProxy();
                        myProxy.Address = new Uri(proxyuri);
                        myProxy.Credentials = new NetworkCredential(ppx.userProxy, ppx.passProxy);
                        //myProxy.Credentials = new NetworkCredential("treinamento.ds1@grupocriar", "Identificar!1");
                        request.Proxy = myProxy;

                    }
                    else
                    {
                        request.Credentials = CredentialCache.DefaultCredentials;
                    }
                }
                else
                {
                    request.Credentials = CredentialCache.DefaultCredentials;
                }

                responseFromServer = "";
                try
                {
                    using (WebResponse response = request.GetResponse())
                    {
                        using (Stream dataStream = response.GetResponseStream())
                        {
                            StreamReader reader = new StreamReader(dataStream);
                            responseFromServer = reader.ReadToEnd();

                            var respMunicipio = JsonConvert.DeserializeObject<ObRespMunicipio>(responseFromServer);
                            return respMunicipio;
                        }
                    }
                }
                catch (WebException ex)
                {
                    responseFromServer = "";
                    if (ex.Response != null)
                    {
                        using (WebResponse response = ex.Response)
                        {
                            Stream dataRs = response.GetResponseStream();
                            using (StreamReader reader = new StreamReader(dataRs))
                            {
                                var msgRet = "";
                                responseFromServer += reader.ReadToEnd();
                                var obj = JsonConvert.DeserializeObject<ObRetorno>(responseFromServer);
                                throw new Exception(obj.codigoRetorno + " - " + obj.mensagem);
                            }
                        }
                    }
                    return null;
                }
            }
            catch (Exception e)
            {
                if (e.Message.Contains("O nome remoto não pôde ser resolvido"))
                {
                    throw new Exception("Não foi realizar conexão com o servidor\r\nVerifique a sua conexão e tente novamente");
                }
                else
                {
                    throw new Exception("Falha na tentativa de conexão ao servidor\r\n" + e.Message);
                }
            }
        }

        public async Task<ObRespImagemAssinatura> IncluiAlteraAssinatura(ObImagem imagem, LogFile log)
        {
            try
            {
                log.write("Iniciando Envio de assinatura 5");
                HttpClientHandler httpClientHandler = new HttpClientHandler();
                var ppx = Funcoes.getUserProxy();
                if (ppx != null && !string.IsNullOrEmpty(ppx.userProxy) && !string.IsNullOrEmpty(ppx.passProxy))
                {
                    if (!string.IsNullOrEmpty(proxyuri))
                    {                      
                        WebProxy myProxy = new WebProxy();
                        myProxy.Address = new Uri(proxyuri);
                        myProxy.Credentials = new NetworkCredential(ppx.userProxy, ppx.passProxy);

                        httpClientHandler = new HttpClientHandler
                        {
                            Proxy = myProxy,
                        };       
                    }                   
                }

                log.write("Iniciando Envio de assinatura 6");
                using (var httpClient = new HttpClient(handler: httpClientHandler, disposeHandler: true))
                {
                    httpClient.Timeout = TimeSpan.FromMilliseconds(20000);
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    };

                    log.write("Iniciando Envio de assinatura 7");
                    string envio = JsonConvert.SerializeObject(imagem.Assinatura, Newtonsoft.Json.Formatting.None,
                                settings);
                    MultipartFormDataContent form = new MultipartFormDataContent();
                    var arq = @".\Assinatura.png";
                    FileStream fileStream = File.OpenRead(arq);
                    log.write("Iniciando Envio de assinatura 8");
                    var streamContent = new StreamContent(fileStream);
                    var fileContent = new ByteArrayContent(streamContent.ReadAsByteArrayAsync().Result);

                    form.Add(new StringContent(imagem.Assinatura.id_candidatos + ""), "id_candidatos");
                    form.Add(new StringContent(imagem.Assinatura.cpf), "cpf");
                    form.Add(new StringContent(imagem.Assinatura.renach), "renach");

                    form.Add(fileContent, "foto", Path.GetFileName(arq));

                    log.write("Iniciando Envio de assinatura 9");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(resptoken.token_type, resptoken.access_token);
                    HttpResponseMessage response = httpClient.PostAsync("https://api.identifikar.com.br/v1/rest/assinatura", form).GetAwaiter().GetResult();
                    var input = await response.Content.ReadAsStringAsync();

                    try
                    {
                        streamContent.Dispose();
                        fileStream.Close();
                        fileStream.Dispose();
                        fileContent.Dispose();

                        File.Delete(arq);
                    }
                    catch (Exception e) { }

                    if (input.Contains("message"))
                    {
                        var respfotof = JsonConvert.DeserializeObject<ObRetornoErro>(input);
                        throw new Exception(respfotof.codigo + " - " + respfotof.message);
                    }

                    log.write("Iniciando Envio de assinatura 10: " + input);
                    var respAssinatura = JsonConvert.DeserializeObject<ObRespImagemAssinatura>(input);
                    log.write("Iniciando Envio de assinatura 11");
                    
                    return respAssinatura;
                }
            }
            catch (WebException ex)
            {
                log.write("FALHA NO ENVIO DE ASSINATURA 1", ex);
                responseFromServer = "";
                if (ex.Response != null)
                {
                    using (WebResponse response = ex.Response)
                    {
                        Stream dataRs = response.GetResponseStream();
                        using (StreamReader reader = new StreamReader(dataRs))
                        {
                            var msgRet = "";
                            responseFromServer += reader.ReadToEnd();
                            var obj = JsonConvert.DeserializeObject<ObRetorno>(responseFromServer);
                            log.write("FALHA NO ENVIO DE ASSINATURA 2: " + obj.codigoRetorno + " - " + obj.mensagem);
                            throw new Exception(obj.codigoRetorno + " - " + obj.mensagem);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                log.write("FALHA NO ENVIO DE ASSINATURA", e);
                if (e.Message.Contains("O nome remoto não pôde ser resolvido"))
                {
                    throw new Exception("Não foi realizar conexão com o servidor\r\nVerifique a sua conexão e tente novamente");
                }
                else
                {
                    throw new Exception("Falha na tentativa de conexão ao servidor\r\n" + e.Message);
                }
            }
            return null;
        }

        public async Task<bool> IncluiAlteraBiometria(ObImagem imagem, Func<ObRespImagemBiometria, int> funcaoResp, LogFile log)
        {
            var input = "";
            try
            {
                log.write("INICIANDO ENVIO DA BIOMETRIA");
                HttpClientHandler httpClientHandler = new HttpClientHandler();
                var ppx = Funcoes.getUserProxy();
                if (ppx != null && !string.IsNullOrEmpty(ppx.userProxy) && !string.IsNullOrEmpty(ppx.passProxy))
                {
                    if (!string.IsNullOrEmpty(proxyuri))
                    {
                        WebProxy myProxy = new WebProxy();
                        myProxy.Address = new Uri(proxyuri);
                        myProxy.Credentials = new NetworkCredential(ppx.userProxy, ppx.passProxy);

                        httpClientHandler = new HttpClientHandler
                        {
                            Proxy = myProxy,
                        };
                    }
                }

                using (var httpClient = new HttpClient(handler: httpClientHandler, disposeHandler: true))
                {
                    httpClient.Timeout = TimeSpan.FromMilliseconds(20000);
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    };

                    string envio = JsonConvert.SerializeObject(imagem.BioCadastro, Newtonsoft.Json.Formatting.None,
                                settings);
                    MultipartFormDataContent form = new MultipartFormDataContent();
                    //var arq = @"..\ImagemNova.wsq";

                    if (imagem.BioCadastro.coletado != 0)
                    {
                        var arq = @".\ImagemNova.wsq";
                        FileStream fileStream = File.OpenRead(arq);
                        var streamContent = new StreamContent(fileStream);
                        var fileContent = new ByteArrayContent(streamContent.ReadAsByteArrayAsync().Result);

                        form.Add(new StringContent(imagem.BioCadastro.id_candidatos + ""), "id_candidatos");
                        form.Add(new StringContent(imagem.BioCadastro.cpf), "cpf");
                        form.Add(new StringContent(imagem.BioCadastro.renach), "renach");
                        form.Add(new StringContent((imagem.BioCadastro.indice - 1) + ""), "indice");
                        form.Add(new StringContent((imagem.BioCadastro.coletado) + ""), "coletado");
                        form.Add(new StringContent(imagem.BioCadastro.minucias + ""), "minucias");

                        form.Add(fileContent, "arquivo", Path.GetFileName(arq));


                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(resptoken.token_type, resptoken.access_token);
                        HttpResponseMessage response = httpClient.PostAsync("https://api.identifikar.com.br/v1/rest/digital", form).GetAwaiter().GetResult();

                        input = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        log.write("RETORNO BIOMETRIAS: " + input);
                        try
                        {
                            streamContent.Dispose();
                            fileStream.Close();
                            fileStream.Dispose();
                            fileContent.Dispose();

                            File.Delete(arq);
                        }
                        catch (Exception e) { }
                        if (input.Contains("message"))
                        {
                            var respfotof = JsonConvert.DeserializeObject<ObRetornoErro>(input);
                            
                        }

                        var respbiometria = JsonConvert.DeserializeObject<List<ObRespImagemBiometria>>(input)[0];

                        

                        funcaoResp(respbiometria);                        
                    }
                    else
                    {
                        form.Add(new StringContent(imagem.BioCadastro.id_candidatos + ""), "id_candidatos");
                        form.Add(new StringContent(imagem.BioCadastro.cpf), "cpf");
                        form.Add(new StringContent(imagem.BioCadastro.renach), "renach");
                        form.Add(new StringContent((imagem.BioCadastro.indice - 1) + ""), "indice");
                        form.Add(new StringContent((imagem.BioCadastro.coletado) + ""), "coletado");
                        form.Add(new StringContent(imagem.BioCadastro.minucias + ""), "minucias");

                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(resptoken.token_type, resptoken.access_token);
                        HttpResponseMessage response = httpClient.PostAsync("https://api.identifikar.com.br/v1/rest/digital", form).GetAwaiter().GetResult();

                        input = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        log.write("RETORNO BIOMETRIAS: " + input);
                        var respbiometria = JsonConvert.DeserializeObject<List<ObRespImagemBiometria>>(input)[0];
                        funcaoResp(respbiometria);
                    }
                    
                    return true;
                }
            }
            catch (WebException ex)
            {
                log.write("FALHA CONEXAO BIOMETRIA",ex);
                responseFromServer = "";
                if (ex.Response != null)
                {
                    using (WebResponse response = ex.Response)
                    {
                        Stream dataRs = response.GetResponseStream();
                        using (StreamReader reader = new StreamReader(dataRs))
                        {
                            var msgRet = "";
                            responseFromServer += reader.ReadToEnd();
                            log.write("FALHA CONEXAO BIOMETRIA: " + responseFromServer);
                            var obj = JsonConvert.DeserializeObject<ObRetorno>(responseFromServer);
                            var ob = new ObRespImagemBiometria();
                            ob.coletado = imagem.BioCadastro.coletado + "";
                            ob.erro = obj.codigoRetorno + " - " + obj.mensagem;
                           
                            funcaoResp(ob);
                            //throw new Exception(obj.codigoRetorno + " - " + obj.mensagem);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                log.write("FALHA CONEXAO BIOMETRIA", e);
                var ob = new ObRespImagemBiometria();
                ob.coletado = imagem.BioCadastro.coletado + "";
                ob.erro = e.Message + " - " + e.StackTrace;
                funcaoResp(ob);
            }
            return false;
        }

        public async Task<ObRespImagemFoto> IncluiAlteraFoto(ObImagem imagem)
        {
            try
            {
                HttpClientHandler httpClientHandler = new HttpClientHandler();
                var ppx = Funcoes.getUserProxy();
                if (ppx != null && !string.IsNullOrEmpty(ppx.userProxy) && !string.IsNullOrEmpty(ppx.passProxy))
                {
                    if (!string.IsNullOrEmpty(proxyuri))
                    {
                        WebProxy myProxy = new WebProxy();
                        myProxy.Address = new Uri(proxyuri);
                        myProxy.Credentials = new NetworkCredential(ppx.userProxy, ppx.passProxy);

                        httpClientHandler = new HttpClientHandler
                        {
                            Proxy = myProxy,
                        };
                    }
                }

                using (var httpClient = new HttpClient(handler: httpClientHandler, disposeHandler: true))
                {
                    httpClient.Timeout = TimeSpan.FromMilliseconds(20000);
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    };

                    string envio = JsonConvert.SerializeObject(imagem.Foto, Newtonsoft.Json.Formatting.None,
                                settings);
                    MultipartFormDataContent form = new MultipartFormDataContent();
                    var arq = @".\ImagemWebCam.jpg";
                    FileStream fileStream = File.OpenRead(arq);
                    var streamContent = new StreamContent(fileStream);
                    var fileContent = new ByteArrayContent(streamContent.ReadAsByteArrayAsync().Result);


                    form.Add(new StringContent(imagem.Foto.id_candidatos + ""), "id_candidatos");
                    form.Add(new StringContent(imagem.Foto.cpf), "cpf");
                    form.Add(new StringContent(imagem.Foto.renach), "renach");

                    form.Add(fileContent, "foto", Path.GetFileName(arq));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(resptoken.token_type, resptoken.access_token);
                    HttpResponseMessage response = httpClient.PostAsync("https://api.identifikar.com.br/v1/rest/foto", form).GetAwaiter().GetResult();

                    fileStream.Close();
                    streamContent.Dispose();
                    fileContent.Dispose();

                    var input = await response.Content.ReadAsStringAsync();
                    if (input.Contains("message"))
                    {
                        var respfotof = JsonConvert.DeserializeObject<ObRetornoErro>(input);
                        throw new Exception(respfotof.codigo + " - " + respfotof.message);
                    }

                    var respfoto = JsonConvert.DeserializeObject<ObRespImagemFoto>(input);
                    return respfoto;
                }

            }
            catch (WebException ex)
            {
                responseFromServer = "";
                if (ex.Response != null)
                {
                    using (WebResponse response = ex.Response)
                    {
                        Stream dataRs = response.GetResponseStream();
                        using (StreamReader reader = new StreamReader(dataRs))
                        {
                            var msgRet = "";
                            responseFromServer += reader.ReadToEnd();
                            var obj = JsonConvert.DeserializeObject<ObRetorno>(responseFromServer);
                            throw new Exception(obj.codigoRetorno + " - " + obj.mensagem);
                        }
                    }
                }
            }
            return null;
        }
        public ObRespImagemAssinatura ConsultaAssinatura(ObImagem imagem, int dado)
        {

            // Variável é o tipo de operação que vai ser feita no método Consulta Dado, que é inserida na classe especificada:
            //dado = 1 => Cpf
            //dado = 2 => id candidato
            //dado = 3 => renach

            string parametros = "";
            if (dado == 1)
                parametros = "?cpf=" + imagem.Assinatura.cpf;
            else if (dado == 2)
                parametros = "?id_candidatos=" + imagem.Assinatura.id_candidatos;
            else if (dado == 3)
                parametros = "?renach=" + imagem.Assinatura.renach;
            WebRequest request = WebRequest.Create("https://api.identifikar.com.br/v1/rest/assinatura" + parametros);
            request.Timeout = 20000;
            //request.Credentials = CredentialCache.DefaultCredentials;
            request.Method = "GET";
            request.Headers["Authorization"] = resptoken.token_type + " " + resptoken.access_token + "";
            string envio = JsonConvert.SerializeObject(imagem.Assinatura);

            var ppx = Funcoes.getUserProxy();
            if (ppx != null && !string.IsNullOrEmpty(ppx.userProxy) && !string.IsNullOrEmpty(ppx.passProxy))
            {

                IWebProxy proxy = request.Proxy;
                if (proxy != null)
                {
                    proxyuri = proxy.GetProxy(request.RequestUri).ToString();
                    WebProxy myProxy = new WebProxy();
                    myProxy.Address = new Uri(proxyuri);
                    myProxy.Credentials = new NetworkCredential(ppx.userProxy, ppx.passProxy);
                    //myProxy.Credentials = new NetworkCredential("treinamento.ds1@grupocriar", "Identificar!1");
                    request.Proxy = myProxy;

                }
                else
                {
                    request.Credentials = CredentialCache.DefaultCredentials;
                }
            }
            else
            {
                request.Credentials = CredentialCache.DefaultCredentials;
            }

            var data = Encoding.UTF8.GetBytes(envio);
            request.ContentType = "application/json";
            responseFromServer = "";
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(dataStream);
                        responseFromServer = reader.ReadToEnd();
                        var obRespImagem = JsonConvert.DeserializeObject<ObRespImagemAssinatura>(responseFromServer);
                        return obRespImagem;
                    }
                }

            }
            catch (WebException ex)
            {
                responseFromServer = "";
                if (ex.Response != null)
                {
                    using (WebResponse response = ex.Response)
                    {
                        Stream dataRs = response.GetResponseStream();
                        using (StreamReader reader = new StreamReader(dataRs))
                        {
                            var msgRet = "";
                            responseFromServer += reader.ReadToEnd();
                            var obj = JsonConvert.DeserializeObject<ObRetorno>(responseFromServer);
                            throw new Exception(obj.codigoRetorno + " - " + obj.mensagem);
                        }
                    }
                }
                return null;
            }

        }
        public ObRespImagemFoto ConsultaFoto(ObImagem imagem, int dado)
        {

            // Variável é o tipo de operação que vai ser feita no método Consulta Dado, que é inserida na classe especificada:
            //dado = 1 => Cpf
            //dado = 2 => id candidato
            //dado = 3 => renach

            string parametros = "";
            if (dado == 1)
                parametros = "?cpf=" + imagem.Foto.cpf;
            else if (dado == 2)
                parametros = "?id_candidatos=" + imagem.Foto.id_candidatos;
            else if (dado == 3)
                parametros = "?renach=" + imagem.Foto.renach;
            WebRequest request = WebRequest.Create("https://api.identifikar.com.br/v1/rest/foto" + parametros);
            request.Timeout = 20000;
            //request.Credentials = CredentialCache.DefaultCredentials;
            request.Method = "GET";
            request.Headers["Authorization"] = resptoken.token_type + " " + resptoken.access_token + "";
            string envio = JsonConvert.SerializeObject(imagem.Foto);

            var ppx = Funcoes.getUserProxy();
            if (ppx != null && !string.IsNullOrEmpty(ppx.userProxy) && !string.IsNullOrEmpty(ppx.passProxy))
            {

                IWebProxy proxy = request.Proxy;
                if (proxy != null)
                {
                    proxyuri = proxy.GetProxy(request.RequestUri).ToString();
                    WebProxy myProxy = new WebProxy();
                    myProxy.Address = new Uri(proxyuri);
                    myProxy.Credentials = new NetworkCredential(ppx.userProxy, ppx.passProxy);
                    //myProxy.Credentials = new NetworkCredential("treinamento.ds1@grupocriar", "Identificar!1");
                    request.Proxy = myProxy;

                }
                else
                {
                    request.Credentials = CredentialCache.DefaultCredentials;
                }
            }
            else
            {
                request.Credentials = CredentialCache.DefaultCredentials;
            }

            var data = Encoding.UTF8.GetBytes(envio);
            request.ContentType = "application/json";
            responseFromServer = "";
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(dataStream);
                        responseFromServer = reader.ReadToEnd();
                        var obRespFoto = JsonConvert.DeserializeObject<ObRespImagemFoto>(responseFromServer);
                        return obRespFoto;
                    }
                }

            }
            catch (WebException ex)
            {
                responseFromServer = "";
                if (ex.Response != null)
                {
                    using (WebResponse response = ex.Response)
                    {
                        Stream dataRs = response.GetResponseStream();
                        using (StreamReader reader = new StreamReader(dataRs))
                        {
                            var msgRet = "";
                            responseFromServer += reader.ReadToEnd();
                            var obj = JsonConvert.DeserializeObject<ObRetorno>(responseFromServer);
                            throw new Exception(obj.codigoRetorno + " - " + obj.mensagem);
                        }
                    }
                }
                return null;
            }
        }
        public List<DigitalBio> ConsultaBiometriaTodas(ObImagem imagem, int dado)
        {

            // Variável é o tipo de operação que vai ser feita no método Consulta Dado, que é inserida na classe especificada:
            //dado = 1 => Cpf
            //dado = 2 => id candidato
            //dado = 3 => renach

            string parametros = "";
            if (dado == 1)
                parametros = "?cpf=" + imagem.BioCadastro.cpf;
            else if (dado == 2)
                parametros = "?id_candidatos=" + imagem.BioCadastro.id_candidatos;
            else if (dado == 3)
                parametros = "?renach=" + imagem.BioCadastro.renach;

            WebRequest request = WebRequest.Create("https://api.identifikar.com.br/v1/rest/digital" + parametros);
            request.Timeout = 20000;
            //request.Credentials = CredentialCache.DefaultCredentials;
            request.Method = "GET";
            request.Headers["Authorization"] = resptoken.token_type + " " + resptoken.access_token + "";

            var ppx = Funcoes.getUserProxy();
            if (ppx != null && !string.IsNullOrEmpty(ppx.userProxy) && !string.IsNullOrEmpty(ppx.passProxy))
            {

                IWebProxy proxy = request.Proxy;
                if (proxy != null)
                {
                    proxyuri = proxy.GetProxy(request.RequestUri).ToString();
                    WebProxy myProxy = new WebProxy();
                    myProxy.Address = new Uri(proxyuri);
                    myProxy.Credentials = new NetworkCredential(ppx.userProxy, ppx.passProxy);
                    //myProxy.Credentials = new NetworkCredential("treinamento.ds1@grupocriar", "Identificar!1");
                    request.Proxy = myProxy;

                }
                else
                {
                    request.Credentials = CredentialCache.DefaultCredentials;
                }
            }
            else
            {
                request.Credentials = CredentialCache.DefaultCredentials;
            }

            request.ContentType = "application/json";
            responseFromServer = "";
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(dataStream);
                        responseFromServer = reader.ReadToEnd();

                        var obRespDigital = JsonConvert.DeserializeObject<List<DigitalBio>>(responseFromServer);

                        return obRespDigital;
                    }
                }

            }
            catch (WebException ex)
            {
                responseFromServer = "";
                if (ex.Response != null)
                {
                    using (WebResponse response = ex.Response)
                    {
                        Stream dataRs = response.GetResponseStream();
                        using (StreamReader reader = new StreamReader(dataRs))
                        {
                            var msgRet = "";
                            responseFromServer += reader.ReadToEnd();
                            var obj = JsonConvert.DeserializeObject<ObRetorno>(responseFromServer);
                            throw new Exception(obj.codigoRetorno + " - " + obj.mensagem);
                        }
                    }
                }
                return null;
            }
        }

        public ObRespImagemBiometria ConsultaBiometriaIndice(ObImagem imagem, int dado, int indice, LogFile log)
        {

            // Variável é o tipo de operação que vai ser feita no método Consulta Dado, que é inserida na classe especificada:
            //dado = 1 => Cpf
            //dado = 2 => id candidato
            //dado = 3 => renach

            string parametros = "";
            if (dado == 1)
                parametros = "?cpf=" + imagem.BioCadastro.cpf;
            else if (dado == 2)
                parametros = "?id_candidatos=" + imagem.BioCadastro.id_candidatos;
            else if (dado == 3)
                parametros = "?renach=" + imagem.BioCadastro.renach;

            parametros += "&indice=" + indice;
            WebRequest request = WebRequest.Create("https://api.identifikar.com.br/v1/rest/digital" + parametros);
            request.Timeout = 20000;
            //request.Credentials = CredentialCache.DefaultCredentials;
            request.Method = "GET";
            request.Headers["Authorization"] = resptoken.token_type + " " + resptoken.access_token + "";
            string envio = JsonConvert.SerializeObject(imagem.Biometrias);

            var ppx = Funcoes.getUserProxy();
            if (ppx != null && !string.IsNullOrEmpty(ppx.userProxy) && !string.IsNullOrEmpty(ppx.passProxy))
            {

                IWebProxy proxy = request.Proxy;
                if (proxy != null)
                {
                    proxyuri = proxy.GetProxy(request.RequestUri).ToString();
                    WebProxy myProxy = new WebProxy();
                    myProxy.Address = new Uri(proxyuri);
                    myProxy.Credentials = new NetworkCredential(ppx.userProxy, ppx.passProxy);
                    //myProxy.Credentials = new NetworkCredential("treinamento.ds1@grupocriar", "Identificar!1");
                    request.Proxy = myProxy;

                }
                else
                {
                    request.Credentials = CredentialCache.DefaultCredentials;
                }
            }
            else
            {
                request.Credentials = CredentialCache.DefaultCredentials;
            }

            var data = Encoding.UTF8.GetBytes(envio);
            request.ContentType = "application/json";
            responseFromServer = "";
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(dataStream);
                        responseFromServer = reader.ReadToEnd();
                        //log.write("Resposta [ConsultaBiometriaIndice]: " + responseFromServer);
                        var obRespDigital = JsonConvert.DeserializeObject<List<ObRespImagemBiometria>>(responseFromServer);
                        return obRespDigital[0];
                    }
                }
            }
            catch (WebException ex)
            {
                responseFromServer = "";
                if (ex.Response != null)
                {
                    using (WebResponse response = ex.Response)
                    {
                        Stream dataRs = response.GetResponseStream();
                        using (StreamReader reader = new StreamReader(dataRs))
                        {
                            var msgRet = "";
                            responseFromServer += reader.ReadToEnd();
                            log.write("FALHA Resposta [ConsultaBiometriaIndice]: " + responseFromServer);
                            var obj = JsonConvert.DeserializeObject<ObRetorno>(responseFromServer);
                            throw new Exception(obj.codigoRetorno + " - " + obj.mensagem);
                        }
                    }
                }
                return null;
            }
            catch(Exception ex)
            {
                log.write("FALHA Metodo [ConsultaBiometriaIndice]: ",ex);
                return null;
            }
        }
    }
}
