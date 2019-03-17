using CamadaClasses;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;

namespace SistemaComercioWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<Categoria> categorias = null;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8080/api/Categoria");
                var resposta = client.GetAsync("");

                resposta.Wait();

                var resultado = resposta.Result;

                if (resultado.IsSuccessStatusCode)
                {
                    var readTask = resultado.Content.ReadAsAsync<List<Categoria>>();
                    readTask.Wait();

                    categorias = readTask.Result;
                }
                else
                {
                    categorias = new List<Categoria>();

                    ModelState.AddModelError(string.Empty, "Não há categorias para exibir");
                }

                return View(categorias);
            }
        }

        public ActionResult CadastrarCategoria(Categoria cat)
        {            
            ModelState.Clear(); // Para evitar a validação no carregamento da página
            return View();
        }

        public ActionResult EditarCategoria(int id)
        {
            List<Categoria> categorias = null;
            Categoria objCategoria = null;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8080/api/Categoria");
                var resposta = client.GetAsync("");

                resposta.Wait();

                var resultado = resposta.Result;

                if (resultado.IsSuccessStatusCode)
                {
                    var readTask = resultado.Content.ReadAsAsync<List<Categoria>>();
                    readTask.Wait();

                    categorias = readTask.Result;

                    objCategoria = categorias.Find(x => x.Idcategoria == id);
                }
                else
                {
                    categorias = new List<Categoria>();

                    ModelState.AddModelError(string.Empty, "Não há categorias para exibir");
                }

                return View(objCategoria);
            }
        }

        [HttpGet]
        public ActionResult ExcluirCategoria(int id)
        {
            List<Categoria> categorias = null;
            Categoria objCategoria = null;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8080/api/Categoria");
                var resposta = client.GetAsync("");

                resposta.Wait();

                var resultado = resposta.Result;

                if (resultado.IsSuccessStatusCode)
                {
                    var readTask = resultado.Content.ReadAsAsync<List<Categoria>>();
                    readTask.Wait();

                    categorias = readTask.Result;

                    objCategoria = categorias.Find(x => x.Idcategoria == id);
                }
                else
                {
                    categorias = new List<Categoria>();

                    ModelState.AddModelError(string.Empty, "Não há categorias para exibir");
                }
            }

            return View(objCategoria);
        }

        [HttpPost]
        public ActionResult RespostaCadastrarCategoria(Categoria cat)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8080/api/Categoria");

                var resposta = client.PostAsJsonAsync("", cat);
                resposta.Wait();

                var statusCode = resposta.Result;

                if (statusCode.IsSuccessStatusCode)
                {
                    ViewBag.resposta = "Categoria registrada com sucesso";
                }
                else
                {
                    ViewBag.resposta = "Erro ao registrar categoria. Contate o administrador";
                }

                return View();
            }
        }

        [HttpPost]
        public ActionResult RespostaEditarCategoria(Categoria cat)
        {
            using (var cliente = new HttpClient())
            {
                cliente.BaseAddress = new Uri("http://localhost:8080/api/");

                Console.WriteLine(cat.Idcategoria);
                var resposta = cliente.PutAsJsonAsync("Categoria?id="+cat.Idcategoria, cat);

                resposta.Wait();

                if (resposta.Result.IsSuccessStatusCode)
                {
                    ViewBag.resposta = "Categoria atualizada com sucesso. Id" + cat.Idcategoria;
                }
                else
                {
                    ViewBag.resposta = "Erro ao atualizar categoria. Contate o administrador";
                }
            }

            return View("RespostaCadastrarCategoria");
        }

        [HttpPost]        
        public ActionResult RespostaExcluirCategoria(int id)
        {
            using (var cliente = new HttpClient())
            {
                cliente.BaseAddress = new Uri("http://localhost:8080/api/");
                var resposta = cliente.DeleteAsync("Categoria?id=" + id);

                resposta.Wait();

                if (resposta.Result.IsSuccessStatusCode)
                {
                    ViewBag.resposta = "Categoria excluída com sucesso";
                }
                else
                {
                    ViewBag.resposta = "Erro ao excluir categoria. Contate o admnistrador";
                }

                return View("RespostaCadastrarCategoria");
            }
        }
    }
}