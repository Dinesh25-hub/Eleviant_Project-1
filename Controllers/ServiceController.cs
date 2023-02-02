using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using User_Application.Data;
using User_Application.Models;
using User_Application.Models.Service;

namespace User_Application.Controllers
{
    public class ServiceController : Controller
    {

        private readonly DataContext datacontext;


        public ServiceController(DataContext datacontext)
        {
            this.datacontext = datacontext;
        }
        public IActionResult Service()
        {
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Add AddRequest)
        {
            var data = new Data_List()
            {
                Id = Guid.NewGuid(),
                Name = AddRequest.Name,
                Age = AddRequest.Age,
                Gender = AddRequest.Gender,
                Email = AddRequest.Email,
                PhoneNo = AddRequest.PhoneNo,
                Salary = AddRequest.Salary,
                Address = AddRequest.Address,
            };
            await datacontext.data_Lists.AddAsync(data);
            await datacontext.SaveChangesAsync();
            return RedirectToAction("Add");
        }


        public async Task<IActionResult> Show()
        {
            var data = await datacontext.data_Lists.ToListAsync();
            return View(data);
        }

        public async Task<IActionResult> Edit(Guid Id)
        {
            var data = await datacontext.data_Lists.FirstOrDefaultAsync(x => x.Id == Id);
            if (data != null)
            {
                var view = new Update_Model()
                {
                    Id = data.Id,
                    Name = data.Name,
                    Age = data.Age,
                    Gender = data.Gender,
                    Email = data.Email,
                    PhoneNo = data.PhoneNo,
                    Salary = data.Salary,
                    Address = data.Address,
                };
                return await Task.Run(() => View("Edit", view));
            }
            return RedirectToAction("Show");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Update_Model model)
        {
            var data = await datacontext.data_Lists.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (data != null)
            {

                data.Id = model.Id;
                data.Name = model.Name;
                data.Age = model.Age;
                data.Gender = model.Gender;
                data.Email = model.Email;
                data.PhoneNo = model.PhoneNo;
                data.Salary = model.Salary;
                data.Address = model.Address;
                datacontext.data_Lists.Update(data);
                await datacontext.SaveChangesAsync();
                return await Task.Run(() => View("Edit", model));
            }
            return RedirectToAction("Show");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Update_Model update)
        {
            var Id = update.Id;
            var data = await datacontext.data_Lists.FirstOrDefaultAsync(x => x.Id == Id);
            if (data != null)
            {
                datacontext.data_Lists.Remove(data);
                await datacontext.SaveChangesAsync();

            }
            return RedirectToAction("Show");
        }

        public IActionResult Import()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Import(ImportVM model)
        {
            using (var stream = new MemoryStream())
            {
                await model.file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowcount = worksheet.Dimension.Rows;
                    for (int row=2; row<=rowcount;row++) 
                    {
                        string age = worksheet.Cells[row, 3].Value.ToString().Trim();
                        string phoneno = worksheet.Cells[row, 6].Value.ToString().Trim();
                        string salary = worksheet.Cells[row, 7].Value.ToString().Trim();
                        
                        var data = new Data_List()
                        {
                            Id = Guid.NewGuid(),
                            Name = worksheet.Cells[row, 2].Value.ToString().Trim(),
                            Age = Int32.Parse(age),
                            Gender = worksheet.Cells[row, 4].Value.ToString().Trim(),
                            Email = worksheet.Cells[row, 5].Value.ToString().Trim(),
                            PhoneNo = long.Parse(phoneno) ,
                            Salary = Int32.Parse(salary),
                            Address = worksheet.Cells[row, 8].Value.ToString().Trim()
                        };
                        await datacontext.data_Lists.AddAsync(data);
                        await datacontext.SaveChangesAsync();
                    }
                }
            }
                return RedirectToAction("Show");
        }
        public IActionResult ExcelExport()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("sheet1");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Id";
                worksheet.Cell(currentRow, 2).Value = "Name";
                worksheet.Cell(currentRow, 3).Value = "Age";
                worksheet.Cell(currentRow, 4).Value = "Gender";
                worksheet.Cell(currentRow, 5).Value = "Email";
                worksheet.Cell(currentRow, 6).Value = "PhoneNo";
                worksheet.Cell(currentRow, 7).Value = "Salary";
                worksheet.Cell(currentRow, 8).Value = "Address";
                foreach (var user in datacontext.data_Lists)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = user.Id.ToString();
                    worksheet.Cell(currentRow, 2).Value = user.Name;
                    worksheet.Cell(currentRow, 3).Value = user.Age;
                    worksheet.Cell(currentRow, 4).Value = user.Gender;
                    worksheet.Cell(currentRow, 5).Value = user.Email;
                    worksheet.Cell(currentRow, 6).Value = user.PhoneNo;
                    worksheet.Cell(currentRow, 7).Value = user.Salary;
                    worksheet.Cell(currentRow, 8).Value = user.Address;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "users.xlsx");
                }
            }
        }
        public IActionResult Search()
        {

            return RedirectToAction("Show");
        }

    }
}
