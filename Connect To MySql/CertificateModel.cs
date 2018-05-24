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

        public void AddNewUserCertificate(string certiLink, string certiCode, DateTime dateFinished, string studentId, string courseEnName, string email, string studentFullName)
        {
            int? userId = GetUserId(studentId);

            if (userId == null)
            {
                userId = CreateNewStudentUser(email, studentId, studentFullName);
            }
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

        //Create new student if not found student in database
        //default phone is null and rollNumber = 1
        private int? CreateNewStudentUser(string email, string studentId, string studentFullName)
        {
            int? userId = null;

            MySqlConnection connection = dbConnect.GetConnection();
            MySqlCommand command = new MySqlCommand("INSERT INTO `funix_certification`.`user`(`user_email`, `user_student_code`, `user_fullname`, `user_role`)  VALUES (@user_email, @user_student_code, @user_fullname, @user_role); select last_insert_id() AS ID;", connection);
            command.Parameters.AddWithValue("@user_email", email);
            command.Parameters.AddWithValue("@user_student_code", studentId);
            command.Parameters.AddWithValue("@user_fullname", studentFullName);
            command.Parameters.AddWithValue("@user_role", 1);
            command.Prepare();

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                userId = Convert.ToInt32(reader.GetString("ID"));
            }

            return userId;
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
            MySqlCommand command = new MySqlCommand("SELECT cc_code FROM certification where cc_name = @courseName;", connection);
            command.Parameters.AddWithValue("@courseName", ccEnName);
            command.Prepare();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                //check if the first column has the value or not(null or not)
                //default first column because select only cc_code in query
                //if has value it mean that, that certificate is for course
                // else it for a subject, so need to get subject code from data base
                if (!reader.IsDBNull(0))
                {
                    ccCode = reader.GetString("cc_code").ToString();
                }
                else
                {
                    ccCode = GetCourseCode(ccEnName);
                }
            }

            dbConnect.CloseConnection(connection);

            return ccCode;
        }

        //get subject code from database by English name of thi subject
        private string GetCourseCode(string ccEnName)
        {
            string courseCode = null;
            MySqlConnection connection = dbConnect.GetConnection();
            MySqlCommand command = new MySqlCommand("SELECT course_code FROM course where course_name = @courseName;", connection);
            command.Parameters.AddWithValue("@courseName", ccEnName);
            command.Prepare();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                    courseCode = reader.GetString("course_code").ToString();
            }

            dbConnect.CloseConnection(connection);

            return courseCode;
        }

        public bool CheckCertificateInDatabase(string ccNumber)
        {
            MySqlConnection connection = dbConnect.GetConnection();
            MySqlCommand command = new MySqlCommand("SELECT * FROM user_cc where cc_code = @ccNumber;", connection);
            command.Parameters.AddWithValue("@ccNumber", ccNumber);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                return true;
            }
            return false;
        }

        public void UpdateUserCertificate(string certiLink, string ccNumber, DateTime date, string studentId, string ccEnName, string email, string studentName)
        {
            int? userId = GetUserId(studentId);
            if (userId == null)
            {
                userId = CreateNewStudentUser(email, studentId, studentName);
            }

            int? ccId = GetCourseId(ccEnName);
            MySqlConnection connection = dbConnect.GetConnection();
            MySqlCommand command = new MySqlCommand(@"UPDATE `funix_certification`.`user_cc`
                                                        SET
                                                        `user_id` = @userId,
                                                        `cc_id` = @ccId,
                                                        `date_issued` = @date,
                                                        `cc_link` = @link
                                                        WHERE `cc_code` = @ccNumber;", connection);
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@ccId", ccId);
            command.Parameters.AddWithValue("@date", date);
            command.Parameters.AddWithValue("@link", certiLink);
            command.Parameters.AddWithValue("@ccNumber", ccNumber);
            command.Prepare();
            command.ExecuteNonQuery();
        }
    }
}
