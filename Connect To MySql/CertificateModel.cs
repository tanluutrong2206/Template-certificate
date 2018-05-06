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

        public void AddNewUserCertificate(string certiLink, string certiCode, string dateFinished, string studentId, string courseName)
        {
            string userId = GetUserId(studentId);
            int? courseId = GetCourseId(courseName);

            MySqlConnection connection = dbConnect.GetConnection();
            MySqlCommand command = new MySqlCommand("INSERT INTO `funix_certification`.`user_cc` VALUES (@user_id, @cc_id, @date_issued, @cc_link, @cc_code);", connection);
            command.Parameters.AddWithValue("@user_id", userId);
            command.Parameters.AddWithValue("@cc_id", courseId);
            command.Parameters.AddWithValue("@date_issued", dateFinished);
            command.Parameters.AddWithValue("@cc_link", certiLink);
            command.Parameters.AddWithValue("@cc_code", certiCode);
            command.Prepare();
            command.ExecuteNonQuery();
            
            dbConnect.CloseConnection(connection);
        }


        //THis method cant return correct courseId because the name of course in english and vietnamese
        //is not match with name in excel file
        private int? GetCourseId(string courseName)
        {
            int? courseId = null;
            MySqlConnection connection = dbConnect.GetConnection();
            MySqlCommand command = new MySqlCommand("SELECT * FROM funix_certification.course where course_name_vn like @courseName;", connection);
            command.Parameters.AddWithValue("@courseName", courseName);
            command.Prepare();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                courseId = Convert.ToInt32(reader.GetString("course_id"));
            }

            dbConnect.CloseConnection(connection);
            return courseId;
        }

        private string GetUserId(string studentId)
        {
            string userId = null;
            MySqlConnection connection = dbConnect.GetConnection();
            MySqlCommand command = new MySqlCommand("select * from user where user.user_student_code=@studentId;", connection);
            command.Parameters.AddWithValue("@studentId", studentId);
            command.Prepare();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                userId = reader.GetString("user_id");
            }

            dbConnect.CloseConnection(connection);
            return userId;
        }
    }
}
