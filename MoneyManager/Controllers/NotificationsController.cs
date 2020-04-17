using MoneyManager.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MoneyManager.Controllers
{
	public class NotificationsController : Controller
	{
		SqlConnection myConnection = new SqlConnection("Server=tcp:circleserver.database.windows.net,1433;Initial Catalog=mediastoredb;Persist Security Info=False;User ID=uladzimir_paliukhovich;Password=Remember_me95;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

		public ActionResult Index()
		{
			return View();
		}

		private async Task<IEnumerable<NotificationDataModel>> GetAllNotificationsAsync()
		{
			await myConnection.OpenAsync();

			SqlCommand selectCommand = new SqlCommand($"SELECT * FROM NOTIFICATIONS", myConnection);

			var myReader = await selectCommand.ExecuteReaderAsync();

			var transactions = new List<NotificationDataModel>();

			while (await myReader.ReadAsync())
			{
				transactions.Add(ReadNotification(myReader));
			}

			myReader.Close();
			myConnection.Close();

			return transactions;
		}

		private NotificationDataModel ReadNotification(SqlDataReader reader)
		{
			var text = reader["Text"];
			byte[] utf8Bytes = Encoding.UTF8.GetBytes(text.ToString().ToCharArray());
			String str2 = Encoding.UTF8.GetString(utf8Bytes);

			return new NotificationDataModel
			{
				Id = Int32.Parse(reader["Id"].ToString()),
				Text = str2,
				OwnerId = Int32.Parse(reader["OwnerId"].ToString()),
				TargetUserId = Int32.Parse(reader["TargetUserId"].ToString()),
				TransactionId = Int32.Parse(reader["TransactionId"].ToString())
			};
		}

		public async Task<IEnumerable<NotificationDataModel>> All()
		{
			return await GetAllNotificationsAsync();
		}

		[HttpGet]
		public async Task<ActionResult> My()
		{
			var email = Request.Headers["X-USERNAME"];
			var token = Request.Headers["X-TOKEN"];

			var user = (await new UsersController().GetAllUsers()).FirstOrDefault(w => w.Email == email);

			if (user == null || user.Token != token)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var userNotificationsData = (await GetAllNotificationsAsync()).Where(w => w.TargetUserId == user.Id);
			var userNotifications = userNotificationsData.Select(Map).ToList();

			return new JsonResult
			{
				Data = userNotifications,
				JsonRequestBehavior = JsonRequestBehavior.AllowGet
			};
		}

		private NotificationUserModel Map(NotificationDataModel data)
		{
			return new NotificationUserModel
			{
				Id = data.Id,
				OwnerId = data.OwnerId,
				Text = data.Text,
				TransactionId = data.TransactionId
			};
		}
	}
}