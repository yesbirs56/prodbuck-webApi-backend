using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiBasicCrud.Data;
using System.Data.Entity;
using WebApiBasicCrud.Models;
using WebApiBasicCrud.Authentication;
using System.Threading;

namespace WebApiBasicCrud.Controllers
{
    public class ProductsController : ApiController
    {   
        private ProductContext _context = new ProductContext();
        
        public HttpResponseMessage GetProduts()
        {
            string username = Thread.CurrentPrincipal.Identity.Name;

            var products = (_context.Products.ToList());

            return Request.CreateResponse(HttpStatusCode.OK, products);
            //return Ok(products);
        }
        public HttpResponseMessage GetProduct (int id)
        {
            var product =  _context.Products.Select(p=> new {p.Id,p.Name,p.NumberInStock,p.CategoryId,p.Price,CategoryName=p.Category.Name }).FirstOrDefault(p => p.Id == id);
            /*_context.Entry(product).Reference(p => p.Category).Load();*/
            if (product == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Product with Id : {id} not Found");
            }
            return Request.CreateResponse(HttpStatusCode.OK, product);
        }
        [BasicAuth]
        public HttpResponseMessage Post([FromBody] Product product)
        {
            string role = Thread.CurrentPrincipal.Identity.Name;
            if (role.ToLower() != "admin")
            {
              return Request.CreateResponse(HttpStatusCode.Unauthorized, "You are not Authorized to do this action");
            }
            try
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                var msg = Request.CreateResponse(HttpStatusCode.Created, product);

                msg.Headers.Location = new Uri(Request.RequestUri.ToString() + product.Id);
                return msg;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            
        }
        [BasicAuth]
        public HttpResponseMessage Delete(int id)
        {
            string role = Thread.CurrentPrincipal.Identity.Name;
            if (role.ToLower() != "admin")
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, "You are not Authorized to do this action");
            }
            try
            {
                Product product = _context.Products.FirstOrDefault(p => p.Id == id);
                if (product == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Product With Id: {id} is not exists or have been deleted already");
                }
                _context.Products.Remove(product);
                _context.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, product);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
           

        }
        [BasicAuth]
        public HttpResponseMessage Put(int id, [FromBody]Product product)
        {
            string role = Thread.CurrentPrincipal.Identity.Name;
            if (role.ToLower() != "admin")
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, "You are not Authorized to do this action");
            }
            try
            {
                Product productInDb = _context.Products.FirstOrDefault(p => p.Id == id);
                if (productInDb == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"The Product With Id: {id} Not Found");
                }
                productInDb.Name = product.Name;
                productInDb.Price = product.Price;
                productInDb.CategoryId = product.CategoryId;
                productInDb.NumberInStock = product.NumberInStock;
                _context.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, productInDb);
            }
            catch(Exception ex)
            {
               return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            

        }
    }
}
