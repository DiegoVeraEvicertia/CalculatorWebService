﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using CalculatorService.client.Models;
using System.IO;
using Newtonsoft.Json;

namespace CalculatorService.client
{
	public class ConexionController
	{
		private string url;
		private string num;
		private int number = 0;
		private char separator;
		private Request requestAdd = new Request();
		private Request requestSub = new Request();
		private Request requestMult = new Request();
		private DivRequest requestDiv = new DivRequest();
		private SqrRequest sqrrequest = new SqrRequest();

		// Methods for user parameters depending selection 
		#region GetAddNumbers
		public Request GetAddNumbers()
		{
			
			requestAdd.nums = new List<int>();
			Console.WriteLine("------- Operation Add -------");
			Console.WriteLine("Write the expression without '='. Write '0' for exit.");
			separator = '+';
			url = "http://localhost:52147/calculator/add";
			try
			{
				do
				{
					Console.WriteLine("Operation:");
					num = Console.ReadLine();
					if (num.Contains("="))
					{
						throw new Exception("You write '=' at the end.");
					}
					else
					{
						string[] numbers = num.Trim().Split(separator);
						foreach (string item in numbers)
						{
							try
							{
								requestAdd.nums.Add(int.Parse(item));

							}
							catch (Exception ex)
							{
								ex = new Exception("Your expression is not valid!");
								Console.ForegroundColor = ConsoleColor.Red;
								Console.WriteLine(ex.Message);
								Console.ForegroundColor = ConsoleColor.Gray;
								GetAddNumbers();
							}
						}
					}
				} while (number != 0);
			}
			catch (Exception e)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(e.Message);
				Console.ForegroundColor = ConsoleColor.Gray;
				GetAddNumbers();
			}

			return requestAdd;
		}
		#endregion
		#region GetSubNumbers
		public Request GetSubtractNumbers()
		{
			requestSub.nums = new List<int>();
			Console.WriteLine("------- Operation Subtract -------");
			Console.WriteLine("Write the expression without '='. Write '0' for exit.");
			separator = '-';
			url = "http://localhost:52147/calculator/sub";
			try
			{
				do
				{
					Console.WriteLine("Operation:");
					num = Console.ReadLine();
					if (num.Contains("="))
					{
						throw new Exception("You write '=' at the end.");
					}
					else
					{
						string[] numbers = num.Trim().Split(separator);
						foreach (string item in numbers)
						{
							try
							{
								requestSub.nums.Add(int.Parse(item));

							}
							catch (Exception ex)
							{
								ex = new Exception("Your expression is not valid!");
								Console.ForegroundColor = ConsoleColor.Red;
								Console.WriteLine(ex.Message);
								Console.ForegroundColor = ConsoleColor.Gray;
								GetSubtractNumbers();
							}
						}
					}
				} while (number != 0);
			}
			catch (Exception e)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(e.Message);
				Console.ForegroundColor = ConsoleColor.Gray;
				GetSubtractNumbers();
			}
			return requestSub;
		}
		#endregion
		#region GetMultNumbers
		public Request GetMultNumbers()
		{
			
			requestMult.nums = new List<int>();
			Console.WriteLine("------- Operation Multiply -------");
			Console.WriteLine("Write the expression without '='. Write '0' for exit.");
			separator = '*';
			url = "http://localhost:52147/calculator/mult";
			try
			{
				do
				{
					Console.WriteLine("Operation:");
					num = Console.ReadLine();
					if (num.Contains("="))
					{
						throw new Exception("You write '=' at the end.");
					}
					else
					{
						string[] numbers = num.Trim().Split(separator);
						foreach (string item in numbers)
						{
							try
							{
								requestMult.nums.Add(int.Parse(item));

							}
							catch (Exception ex)
							{
								ex = new Exception("Your expression is not valid!");
								Console.ForegroundColor = ConsoleColor.Red;
								Console.WriteLine(ex.Message);
								Console.ForegroundColor = ConsoleColor.Gray;
								GetMultNumbers(); 
							}
						}
					}
				} while (number != 0);
			}
			catch (Exception e)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(e.Message);
				Console.ForegroundColor = ConsoleColor.Gray;
				GetMultNumbers();
			}

			return requestMult;
		}
		#endregion
		#region GetDivNumbers
		public DivRequest GetDivNumbers()
		{
			
			Console.WriteLine("------- Operation Divide -------");
			url = "http://localhost:52147/calculator/div";
			try
			{
				Console.WriteLine("Number dividend: ");
				requestDiv.num1 = int.Parse(Console.ReadLine());

				Console.WriteLine("Number divisor: ");
				requestDiv.num2 = int.Parse(Console.ReadLine());
			}
			catch (Exception ex)
			{
				ex = new Exception("Your expression is not valid!");
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(ex.Message);
				Console.ForegroundColor = ConsoleColor.Gray;
				GetDivNumbers(); 
			}
			return requestDiv;
		}
		#endregion
		#region GetSqrNumbers
		public SqrRequest GetSqrNumbers()
		{
			
			Console.WriteLine("------- Operation Square -------");
			url = "http://localhost:52147/calculator/sqr";

			try
			{
				Console.WriteLine("Number: ");
				sqrrequest.num = int.Parse(Console.ReadLine());
			}
			catch (Exception ex)
			{
				ex = new Exception("Your expression is not valid!");
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(ex.Message);
				Console.ForegroundColor = ConsoleColor.Gray;
				GetSqrNumbers(); 
			}

			return sqrrequest;

		}

		#endregion

		// Connection with the server
		public void Connection(string op) {

			// Classes for sending the user parameters and get the server response
			Request request = new Request();
			DivRequest divRequest = new DivRequest();
			SqrRequest sqrRequest = new SqrRequest();
			Response response = new Response();
			string jsonRequest = "";

			#region CallGetOperations
			switch (op)
			{
				case "add":
				case "1":
					request = GetAddNumbers();
					break;
				case "subtract":
				case "2":
					request = GetSubtractNumbers();
					break;
				case "multiply":
				case "3":
					request = GetMultNumbers();
					break;
				case "divide":
				case "4":
					divRequest = GetDivNumbers();
					break;
				case "sqr":
				case "5":
					sqrRequest = GetSqrNumbers();
					break;
			}
			#endregion

			// Opens the connection with the defined url depending the method selection 
			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
				req.Method = "POST";
				req.ContentType = "application/json";

				// Opens a stream,converts the request to Json and send it 
				using (StreamWriter sw = new StreamWriter(req.GetRequestStream()))
				{
					switch (op)
					{
						case "subtract":
						case "multiply":
						case "add":
						case "1":
						case "2":
						case "3":
							jsonRequest = JsonConvert.SerializeObject(request);
							break;
						case "divide":
						case "4":
							jsonRequest = JsonConvert.SerializeObject(divRequest);
							break;
						case "sqr":
						case "5":
							jsonRequest = JsonConvert.SerializeObject(sqrRequest);
							break;
					}
					sw.Write(jsonRequest);
					sw.Close();
				}

				// Get the server response and transform the Json to the class response
				HttpWebResponse Response = (HttpWebResponse)req.GetResponse();
				using (StreamReader sr = new StreamReader(Response.GetResponseStream(), Encoding.UTF8))
				{
					response = JsonConvert.DeserializeObject<Response>(sr.ReadToEnd());
					sr.Close();
					Response.Close();
				}

				Console.WriteLine("The server responds: ");
				Console.WriteLine($"{response.operation} = {response.Total}");

		}
	}
}