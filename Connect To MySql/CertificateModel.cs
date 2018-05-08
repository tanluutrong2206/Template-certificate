using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect_To_MySql
{
    public class CertificateModel
    {
        private DbConnect dbConnect;
        public CertificateModel()
        {
            dbConnect = new DbConnect();
        }

        public void AddNewUserCertificate(string certiLink, string certiCode, DateTime dateFinished, string studentId, string courseEnName)
        {
            int? userId = GetUserId(studentId);
            int? courseId = GetCourseId(courseEnName);

            MySqlConnection connection = dbConnect.GetConnection();
            MySqlCommand command = new MySqlCommand("INSERT INTO `funix_certification`.`user_cc`(`user_id`, `cc_id`, `date_issued`, `cc_link`, `cc_code`) VALUES (@user_id, @cc_id, @date_issued, @cc_link, @cc_code);", connection);
            command.Parameters.AddWithValue("@user_id", userId);
            command.Parameters.AddWithValue("@cc_id", courseId);
            command.Parameters.AddWithValue("@date_issued", dateFinished);
            command.Parameters.AddWithValue("@cc_link", certiLink);
            command.Parameters.AddWithValue("@cc_code", certiCode);
            command.Prepare();
            command.ExecuteNonQuery();
            
            dbConnect.CloseConnection(connection);
        }

        private int? GetCourseId(string courseEnName)
        {
            int? courseId = null;
            MySqlConnection connection = dbConnect.GetConnection();
            MySqlCommand command = new MySqlCommand("SELECT * FROM certification where cc_name = @courseName;", connection);
            command.Parameters.AddWithValue("@courseName", courseEnName);
            command.Prepare();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                courseId = Convert.ToInt32(reader.GetString("cc_id"));
            }

            dbConnect.CloseConnection(connection);
            return courseId;
        }

        private int? GetUserId(string studentId)
        {
            int? userId = null;
            MySqlConnection connection = dbConnect.GetConnection();
            MySqlCommand command = new MySqlCommand("select * from user where user.user_student_code=@studentId;", connection);
            command.Parameters.AddWithValue("@studentId", studentId);
            command.Prepare();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                userId = Convert.ToInt32(reader.GetString("user_id"));
            }

            dbConnect.CloseConnection(connection);
            return userId;
        }

        public string GetCcCode(string ccEnName)
        {
            string ccCode = null;
            MySqlConnection connection = dbConnect.GetConnection();
            MySqlCommand command = new MySqlCommand("SELECT * FROM certification where cc_name = @courseName;", connection);
            command.Parameters.AddWithValue("@courseName", ccEnName);
            command.Prepare();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                ccCode = reader.GetString("cc_code").ToString();
            }

            dbConnect.CloseConnection(connection);
            return ccCode;
        }
    }
}
