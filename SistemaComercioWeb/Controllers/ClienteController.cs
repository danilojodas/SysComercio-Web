using CamadaClasses;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;

namespace SistemaComercioWeb.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult CadastrarCliente(Cliente cli)
        {
            // É necessário passar um objeto à View para que as validações funcionem
            ModelState.Clear();
            return View();
        }

        public ActionResult EditarCliente(int id)
        {
            List<Cliente> listaCliente;
            Cliente cli = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8080/api/Cliente");
                var resposta = client.GetAsync("");

                resposta.Wait();

                if(resposta.Result.IsSuccessStatusCode)
                {
                    var readTask = resposta.Result.Content.ReadAsAsync<List<Cliente>>();
                    readTask.Wait();

                    listaCliente = readTask.Result;

                    cli = listaCliente.Find(x => x.IdCliente == id);
                }
                else
                {
                    listaCliente = new List<Cliente>();
                    ModelState.AddModelError(String.Empty,"Erro ao editar o cliente. Contate o administrador para maiores informações");
                }

                return View(cli);
            }                
        }

        public ActionResult ConsultarCliente()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ConsultarCliente(string nomeCliente)
        {
            List<Cliente> listaClientes = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8080/api/");

                var resposta = client.GetAsync("Cliente/" + nomeCliente);

                resposta.Wait();

                if(resposta.Result.IsSuccessStatusCode)
                {
                    var readTask = resposta.Result.Content.ReadAsAsync<List<Cliente>>();
                    readTask.Wait();

                    listaClientes = readTask.Result;
                }
                else
                {
                    listaClientes = new List<Cliente>();

                    ModelState.AddModelError(String.Empty,"Ocorreu um erro ao consultar o cliente. Contate o administrador para maiores informações");
                }
            }
            return View(listaClientes);
        }

        [HttpPost]
        public ActionResult RespostaCadastrarCliente(Cliente cli)
        {
            using (var client = new HttpClient())
            {
                // Endereço da WEB API de clientes
                client.BaseAddress = new Uri("http://localhost:8080/api/Cliente");

                // Variável que recebe a resposta do POST
                var resposta = client.PostAsJsonAsync("", cli);

                // Verifica se o código de retorno é de sucesso
                if (resposta.Result.IsSuccessStatusCode)
                {
                    ViewBag.resposta = "Cliente cadastrado com sucesso";
                }
                else
                {
                    ViewBag.resposta = "Erro ao cadastrar cliente. Entre em contato com o administrador";
                }

                return View();
            }
        }

        [HttpPost]
        public ActionResult RespostaEditarCliente(Cliente cli)
        {
            using (var client = new HttpClient())
            {                
                client.BaseAddress = new Uri("http://localhost:8080/api/");
                var resposta = client.PutAsJsonAsync("Cliente?id="+cli.IdCliente, cli);
                resposta.Wait();

                if (resposta.Result.IsSuccessStatusCode)
                {
                    ViewBag.resposta = "Cliente atualizado com sucesso";
                }
                else
                {
                    ViewBag.resposta = "Erro ao atualizar cliente. Contate o administrador para maiores informações";
                }

                return View();
            }                
        }
    }
}