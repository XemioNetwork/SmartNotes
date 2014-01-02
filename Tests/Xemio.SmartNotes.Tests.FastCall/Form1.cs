using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xemio.SmartNotes.Abstractions.Authorization;

namespace Xemio.SmartNotes.Tests.FastCall
{
	public partial class Form1 : Form
	{
        private readonly HttpClient _client = new HttpClient();

		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			string authorizationHash = AuthorizationHash.Create(tbxUsername.Text, tbxPassword.Text, tbxContent.Text);

			var request = new HttpRequestMessage(new HttpMethod(cbxKind.Text), "http://localhost/" + tbxAddress.Text)
			{
				Headers =
				{
					Authorization = new AuthenticationHeaderValue("Xemio", string.Format("{0}:{1}", tbxUsername.Text, authorizationHash)),
				}
			};

            if (string.IsNullOrWhiteSpace(tbxContent.Text) == false)
                request.Content = new StringContent(tbxContent.Text, Encoding.UTF8, "application/json");

		    _client.SendAsync(request).ContinueWith(this.HandleResponse);
		}

	    private void HandleResponse(Task<HttpResponseMessage> responseTask)
	    {
	        if (!responseTask.IsCompleted)
	        {
	            MessageBox.Show("Error with the request task.");
	            return;
	        }

	        string response = responseTask.Result.Content.ReadAsStringAsync().Result;
	        MessageBox.Show(response);
	    }
	}
}
