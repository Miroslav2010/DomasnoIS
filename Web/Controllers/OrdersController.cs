using GemBox.Document;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;
using Services.Interface;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
namespace Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;


        public OrdersController(IOrderService orderService)
        {
            this._orderService = orderService;
        }

        public IActionResult Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = _orderService.getAllOrders().Where(z => z.UserId == userId).ToList();
            return View(orders);
        }

        public IActionResult Details(Guid id)
        {
            var result = _orderService.getOrderDetails(new Domain.DomainModels.BaseEntity
            {
                Id = id
            });


            return View(result);
        }

        public FileContentResult CreateInvoice(Guid id)
        {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            var result = _orderService.getOrderDetails(_orderService.getAllOrders().First(z => z.Id == id));

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");
            var document = DocumentModel.Load(templatePath);


            document.Content.Replace("{{OrderNumber}}", result.Id.ToString());
            document.Content.Replace("{{UserName}}", result.User.UserName);

            StringBuilder sb = new StringBuilder();

            var totalPrice = 0.0;

            foreach (var item in result.Tickets)
            {
                totalPrice += item.Quantity * item.Ticket.Price;
                sb.AppendLine(item.Ticket.Name + " with quantity of: " + item.Quantity + " and price of: " + item.Ticket.Price + "$");
            }

            document.Content.Replace("{{ProductList}}", sb.ToString());
            document.Content.Replace("{{TotalPrice}}", totalPrice.ToString() + "$");

            var stream = new MemoryStream();

            document.Save(stream, new PdfSaveOptions());

            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ExportInvoice.pdf");
        }
    }
}