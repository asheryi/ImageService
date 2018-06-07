using Newtonsoft.Json.Linq;
using ShaeredResources.Comunication;
using SharedResources.Commands;
using SharedResources.Communication;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Model;
using WebApplication2.Model.Communication;
using WebApplication2.Models;
using WebApplication2.Models.Logging;
using WebApplication2.Models.Models;

namespace WebApplication2.Controllers
{
    public class FirstController : Controller
    {
        static List<Employee> employees = new List<Employee>()
        {
          new Employee  { FirstName = "Moshe", LastName = "Aron", Email = "Stam@stam", Salary = 10000, Phone = "08-8888888" },
          new Employee  { FirstName = "Dor", LastName = "Nisim", Email = "Stam@stam", Salary = 2000, Phone = "08-8888888" },
          new Employee   { FirstName = "Mor", LastName = "Sinai", Email = "Stam@stam", Salary = 500, Phone = "08-8888888" },
          new Employee   { FirstName = "Dor", LastName = "Nisim", Email = "Stam@stam", Salary = 20, Phone = "08-8888888" },
          new Employee   { FirstName = "Dor", LastName = "Nisim", Email = "Stam@stam", Salary = 700, Phone = "08-8888888" }
        };

        // static List<Log> logs = new List<Log>();
        //{
        //  new Log(MessageTypeEnum.INFO,"Hey"), new Log(MessageTypeEnum.INFO,"Baye")
        //    , new Log(MessageTypeEnum.WARNING,"Hello"), new Log(MessageTypeEnum.INFO,"Yes")
        //};
        static Student student = new Student("avi", 1234);

        static Client client = new Client();

        static IMessageGenerator messGenerator = new CommunicationMessageGenerator();
        static SettingsModel settingsModel = new SettingsModel();
        static LogsModel logsModel = new LogsModel();
        static IMessageHandler messHandler; // MAYBE NOT


        public FirstController()
        {
            if (!client.Connected)
            {
                messHandler = new CommunicationMessageHandler();

                messHandler.RegisterFuncToEvent(CommandEnum.GetAllLogsCommand, logsModel.recieveLogs);
                messHandler.RegisterFuncToEvent(CommandEnum.SendLog, logsModel.recieveOneLog);

                messHandler.RegisterFuncToEvent(CommandEnum.GetConfigCommand, settingsModel.recieveSettings);
                messHandler.RegisterFuncToEvent(CommandEnum.CloseHandlerCommand, settingsModel.removeHandler);


                client.messageHandler = messHandler;

                client.Start();
                client.Recieve();
            } else
            {
                messHandler = null;
            }



        }


        // GET: First
        public ActionResult Config()
        {
            return View(settingsModel.Settings);
        }

        [HttpGet]
        public ActionResult AjaxView()
        {
            return View();
        }

        [HttpGet]
        public JObject GetEmployee()
        {
            JObject data = new JObject();
            data["FirstName"] = "Kuky";
            data["LastName"] = "Mopy";
            return data;
        }

        [HttpPost]
        public JObject GetEmployee(string name, int salary)
        {
            foreach (var empl in employees)
            {
                if (empl.Salary > salary || name.Equals(name))
                {
                    JObject data = new JObject();
                    data["FirstName"] = empl.FirstName;
                    data["LastName"] = empl.LastName;
                    data["Salary"] = empl.Salary;
                    return data;
                }
            }
            return null;
        }

        // GET: First/Details
        public ActionResult Logs(string filter="n")
        {
            return View(logsModel.filterLogsBy(filter));
        }

        public void getAllLogs()
        {
            client.Send(messGenerator.Generate(CommandEnum.GetAllLogsCommand,""));
        }


        public ActionResult RemoveHandlerq(string id)
        {
            Debug.WriteLine("Remove Handler");
            List<Log> logs = new List<Log>();
            return View(logs);
        }
        // GET: First/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: First/Create
        [HttpPost]
        public ActionResult Create(Employee emp)
        {
            try
            {
                employees.Add(emp);
                student.add(emp.FirstName);
                return RedirectToAction("Logs");
            }
            catch
            {
                return View();
            }
        }

        // GET: First/Edit/5
        //public ActionResult RemoveHandler(string handler)
        //{
        //    Debug.WriteLine("Editttttttt");
        //    DirectoryDetails dd = new DirectoryDetails(handler);
        //    client.askRemoveHandler(dd);
           
        //    return View();
            
        //}
        public ActionResult RemoveHandler(string x)
        {
            Debug.WriteLine("Editttttttt");
            //DirectoryDetails dd = new DirectoryDetails(handler);
            //client.askRemoveHandler(dd);

            return View();
        }
        // POST: First/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Employee empT)
        {
            try
            {
                foreach (Employee emp in employees)
                {
                    if (emp.ID.Equals(id))
                    {
                        emp.copy(empT);
                        return RedirectToAction("Index");
                    }
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Error");
            }
        }

        // GET: First/Delete/5
        public ActionResult Delete(int id)
        {
            int i = 0;
            foreach (Employee emp in employees)
            {
                if (emp.ID.Equals(id))
                {
                    employees.RemoveAt(i);
                    return RedirectToAction("Logs");
                }
                i++;
            }
            return RedirectToAction("Config");

        }
    }
}
