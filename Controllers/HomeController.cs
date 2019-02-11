using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dojodachi.Models;
using Microsoft.AspNetCore.Http;

namespace Dojodachi.Controllers
{
    public class HomeController : Controller
    {
        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            bool result = false;
            int? count = HttpContext.Session.GetInt32("count");
            if (count == null){
                HttpContext.Session.SetInt32("count", 1);
                HttpContext.Session.SetInt32("happiness", 20);
                HttpContext.Session.SetInt32("fullness", 20);
                HttpContext.Session.SetInt32("energy", 50);
                HttpContext.Session.SetInt32("meal", 3);
                HttpContext.Session.SetString("message", "");
            }

            TempData["Happiness"]  = HttpContext.Session.GetInt32("happiness");
            TempData["Fullness"] = HttpContext.Session.GetInt32("fullness");
            TempData["Energy"] = HttpContext.Session.GetInt32("energy");
            TempData["Meal"] = HttpContext.Session.GetInt32("meal");
            TempData["Message"] = HttpContext.Session.GetString("message");
            int happy = (int)HttpContext.Session.GetInt32("happiness");
            int full = (int)HttpContext.Session.GetInt32("fullness");
            int energy = (int)HttpContext.Session.GetInt32("energy");
            if (happy <= 0 || full <= 0){

                HttpContext.Session.SetString("message", "Game Over your Dojodachi Died!");
                TempData["Message"] = HttpContext.Session.GetString("message");
                result = true;
            }
            else if (happy >= 100 && full >= 100 && energy >= 100){
                HttpContext.Session.SetString("message", "You Win! Your Dojodachi is satified!");
                TempData["Message"] = HttpContext.Session.GetString("message");
                result = true;
            }


            return View("Index", result);
        }
        
        [HttpPost("feed")]
        public IActionResult Feed(){
            int feed = (int)HttpContext.Session.GetInt32("meal");

            if (feed <= 0){
                HttpContext.Session.SetString("message", "Your Dojodachi can't gain without having some feed");

            }
            else{
                Random rand = new Random();
                int chance = rand.Next(4);
                feed -= 1;
                if (chance == 0){
                    HttpContext.Session.SetInt32("meal", feed);
                    HttpContext.Session.SetString("message", $"Your Dojodachi ate but does not feel anything");

                }
                else{
                    int full = (int)HttpContext.Session.GetInt32("fullness");
                    int gainer = rand.Next(5,11);
                    full += gainer;
                    HttpContext.Session.SetInt32("fullness", full);
                    HttpContext.Session.SetInt32("meal", feed);
                    HttpContext.Session.SetString("message", $"Your Dojodachi have gained {gainer} of Fullness");
                }
            }

            return RedirectToAction("Index");
        }


        [HttpPost("play")]
        public IActionResult Play(){
            int energy = (int)HttpContext.Session.GetInt32("energy");

            if (energy < 5){
                HttpContext.Session.SetString("message", "Your Dojodachi can't gain without having some energy");

            }
            else{
                Random rand = new Random();
                int chance = rand.Next(4);
                energy -= 5;
                if (chance == 0){
                    HttpContext.Session.SetInt32("energy", energy);
                    HttpContext.Session.SetString("message", $"Your Dojodachi does not want to play");

                }
                else{
                    int happy = (int)HttpContext.Session.GetInt32("happiness");
                    int gainer = rand.Next(5,11);
                    happy += gainer;
                    HttpContext.Session.SetInt32("energy", energy);
                    HttpContext.Session.SetInt32("happiness", happy);
                    HttpContext.Session.SetString("message", $"Your Dojodachi have gained {gainer} of Happiness");
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost("work")]
        public IActionResult Work(){
            Random rand = new Random();
            int gainer = rand.Next(1,4);
            int meal = (int)HttpContext.Session.GetInt32("meal");
            int energy = (int)HttpContext.Session.GetInt32("energy");
            meal += gainer;
            energy -= 5;
            HttpContext.Session.SetInt32("meal", meal);
            HttpContext.Session.SetInt32("energy", energy);
            HttpContext.Session.SetString("message", $"You have gained {gainer} meal points!");

            return RedirectToAction("Index");
        }

        [HttpPost("sleep")]
        public IActionResult Sleep(){
            int energy = (int)HttpContext.Session.GetInt32("energy");
            
            int happy = (int)HttpContext.Session.GetInt32("happiness");
            int full = (int)HttpContext.Session.GetInt32("fullness");
            energy += 15;
            happy -= 5;
            full -= 5;
            HttpContext.Session.SetInt32("energy", energy);
            HttpContext.Session.SetInt32("happiness", happy);
            HttpContext.Session.SetInt32("fullness", full);
            HttpContext.Session.SetString("message", "Your Dojodachi has regain 15 energy points!");

            return RedirectToAction("Index");
        }

        [HttpPost("reset")]
        public IActionResult Reset()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
    
}
