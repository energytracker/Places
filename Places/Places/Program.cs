using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Places
{
    class Program
    {
        static void Main(string[] args)
        {
            string select = "";
            DataTable DT_Nodos = new DataTable();
            using (SqlConnection conn = new SqlConnection(DBLibrary.condb2))
            using (SqlCommand command = new SqlCommand(select, conn))
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
            dataAdapter.Fill(DT_Nodos);
            MapsApi_Model ModelApi = new MapsApi_Model();
            foreach (DataRow row in DT_Nodos.Rows)
            {
                string IdNodo = row["IdNodo"].ToString();
                string NombreNodo = row["NombreNodo"].ToString();
                string ZonaCarga = row["ZonaCarga"].ToString();

                

                HttpWebRequest request = WebRequest.Create(GooglePlaces_Request) as HttpWebRequest;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string resp = reader.ReadToEnd();
                    var obj = JsonConvert.DeserializeObject<object>(resp);
                    string objdm = Convert.ToString(obj);
                    ModelApi = JsonConvert.DeserializeObject<MapsApi_Model>(objdm);

                    string direccion = ModelApi.Results[0].FormattedAddress.ToString();
                    string placeId = ModelApi.Results[0].PlaceId;
                    string lat = Convert.ToString(ModelApi.Results[0].Geometry.Location.Lat);
                    string lng = Convert.ToString(ModelApi.Results[0].Geometry.Location.Lng);


                }
            }

            string s = "";
        }
        
    }
}
