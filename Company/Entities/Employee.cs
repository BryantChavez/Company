using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using ADOX;

namespace Company.Entities
{
    public class Employee
    {
        public String Id { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String BirthDate { get; set; }
        
        
        //Adds a user to the DB
        public Boolean Add(Employee e)
        {
            Boolean result = false;
            OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; Data Source=Master.mdb");
            try
            { 
                string strSql = "insert into Employees(Id,FirstName,LastName,BirthDate) Values(" + e.Id + ",'" + e.FirstName + "','" + e.LastName + "','" + e.BirthDate + "')";
                OleDbCommand cmd = con.CreateCommand();
                con.Open();
                cmd.CommandText = strSql;
                cmd.ExecuteNonQuery();
                MessageBox.Show("User Added!");
                result = true;
            }catch(Exception ex)
            {
                result = false;
                MessageBox.Show("User not Added!");
            }
            con.Close();
            return result;

        }

        //Checks in the db if id exists
        public Boolean IfIdExists(int id)
        {
            Boolean result = true; 
            OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; Data Source=Master.mdb");
            try
            {
                string strSql = "SELECT Id FROM Employees WHERE Id=" + id;
                con.Open();
                OleDbCommand cmd = new OleDbCommand(strSql, con);
                cmd.CommandType = CommandType.Text;
                OleDbDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {//If id exists.
                    MessageBox.Show("Id already exists, use another Id. User not added.");
                }
                else
                {//If id doesnt exist.
                    result = false;
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error.");
            }
            con.Close();
            return result;

        }

        //Method that gets all the list of the employees
        public List<Employee> GetEmployees()
        {
            List<Employee> employees = new List<Employee>();
            try
            {
                string strSql = @"Select * From Employees";
                OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; Data Source=Master.mdb");
                con.Open();
                OleDbCommand cmd = new OleDbCommand(strSql, con);
                cmd.CommandType = CommandType.Text;
                OleDbDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Employee em = new Employee();
                        em.Id = dr.GetValue(0).ToString();
                        em.FirstName = dr.GetValue(1).ToString();
                        em.LastName = dr.GetValue(2).ToString();
                        em.BirthDate = dr.GetValue(3).ToString();

                        employees.Add(em);
                    }
                }
                dr.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error.");
            }
            return employees;
        }

        //Method that will convert the mdb to a txt file
        public Boolean ConvertToTxt()
        {
            Boolean result = false;
            string FileName = @"Output_PipeDelimited.txt";

            try
            {
                // Check if file already exists. If yes, delete it.     
                if (File.Exists(FileName))
                {
                    File.Delete(FileName);
                }

                List<Employee> employees = new List<Employee>();
                employees = GetEmployees();
                using (StreamWriter sw = File.CreateText(FileName))
                {
                    for(int i = 0; i<employees.Count; i++)
                    {
                        sw.WriteLine(employees[i].Id +"|"+employees[i].FirstName+"|"+employees[i].LastName+"|"+employees[i].BirthDate);
                    }
                }
                result = true;
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
            return result;
        }

        //Method that will convert the mdb to xml file
        public Boolean ConvertToXml()
        {
            Boolean result = false;
            try
            {
                XmlWriter xmlWriter = XmlWriter.Create("Output_XML.xml");

                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("Employees");

                List<Employee> employees = GetEmployees();
                for(int i = 0; i < employees.Count; i++)
                {
                    //<Employee Id>
                    xmlWriter.WriteStartElement("Employee");
                    xmlWriter.WriteAttributeString("Id", employees[i].Id);

                    //<LastName>
                    xmlWriter.WriteStartElement("LastName");
                    xmlWriter.WriteString(employees[i].LastName);
                    xmlWriter.WriteEndElement();

                    //<FirstName>
                    xmlWriter.WriteStartElement("FirstName");
                    xmlWriter.WriteString(employees[i].FirstName);
                    xmlWriter.WriteEndElement();

                    //<BirthDate>
                    xmlWriter.WriteStartElement("BirthDate");
                    xmlWriter.WriteString(employees[i].BirthDate);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteEndDocument();
                xmlWriter.Close();
                result = true;
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
            return result;
        }

    }
}
