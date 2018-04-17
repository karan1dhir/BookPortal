using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Backend.Controllers
{
    public class HomeController : ApiController
    {

        DepartmentEntitiesDbCon db = new DepartmentEntitiesDbCon();
        public HomeController()
        {

        }
        [HttpGet]
        public IHttpActionResult GetDetails()
        {
            try
            {

                List<tblDepartment> list = db.tblDepartments.ToList();
                if (list != null)
                {
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, list));
                }
                else throw new Exception("List is null");
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }

        }

        public IHttpActionResult GetBooks()
        {
            try
            {
                List<tblDepartment> listbooks = db.tblDepartments.ToList();
                var result = (from dp in db.tblDepartments
                              join book in db.tblBookDetails on dp.ProductId equals book.BookId
                              select new
                              {
                                  dp.ProductType,
                                  book.BookName,
                                  book.BookAuthor,
                                  book.id,
                                  book.BookQuantity,
                                  book.BookPrice
                              }).ToList();

                if (listbooks != null)
                {
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, result));
                }
                else throw new Exception("List is null");
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }
        public IHttpActionResult GetCartDetails()
        {
            try
            {
                List<tblCartDetail> cartDetails = db.tblCartDetails.ToList();
                if (cartDetails != null)
                {
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, cartDetails));
                }
                else throw new Exception("List is null");
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }
        [HttpPost]
        public HttpResponseMessage AddDetails([FromBody]tblCartDetail e)
        {

            try
            {
                db.tblCartDetails.Add(e);
                db.SaveChanges();
                var message = Request.CreateResponse(HttpStatusCode.Created, e);
                message.Headers.Location = new Uri(Request.RequestUri + e.id.ToString());
                return message;
            }

            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.ToString());
            }
        }
      [HttpDelete]
        public IHttpActionResult DeletefromCart(int id)
        {
            try
            {
                var entity = db.tblCartDetails.FirstOrDefault(cart => cart.id == id);
                if (entity == null)
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Item with this id is not found"));
                }
                else
                {
                    db.tblCartDetails.Remove(entity);
                    db.SaveChanges();
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK));
                }
              
            }
            catch(Exception ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest,ex.Message));
            }
        }
        [HttpPut]
        public IHttpActionResult UpdatetoCart(int id, [FromBody]tblCartDetail c)
        {
            try {

                var entity = db.tblCartDetails.FirstOrDefault(cart => cart.id == id);
                if (entity == null)
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Item with this id is not found"));
                }
                else
                {
                    entity.BookQuantityUser = c.BookQuantityUser;
                    db.SaveChanges();
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK,entity));
                }
              }
            catch(Exception ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message));
            }
        }
    }
}
