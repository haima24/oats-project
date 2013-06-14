using Microsoft.AspNet.SignalR;
using OATS_Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OATS_Capstone.Hubs
{
    public class GeneralHub : Hub
    {
        //public void DeActiveTest(int testid)
        //{
        //    Clients.All.R_deactivetest("Tu", "akai777@gmail.com");
        //}
    }
    public class ChatHub : Hub
    {
        public void Send(string name, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(name, message);
        }
    }
}