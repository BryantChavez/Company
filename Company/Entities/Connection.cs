using ADOX;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Company.Entities
{
    public class Connection
    {
        //Checks if db exists
        public Boolean IfDbExists()
        {
            Boolean result = false;
            OleDbConnection myConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; Data Source=Master.mdb");
            try
            {
                myConnection.Open();
                result = true;
            }
            catch(Exception ex)
            {

                result = false;

            }
            myConnection.Close();
            if(result == false)
            {
                CreateDB();
            }
            return result;
        }

        //Creates the db in ->...\source\repos\Company\Company\bin\Debug
        public Boolean CreateDB()
        {
            Boolean result = false;
            try
            {
                ADOX.Catalog cat = new ADOX.Catalog();
                String File = "Master.mdb";
                cat.Create("Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + File + ";" + "Jet OLEDB:Engine Type=5");
                MessageBox.Show("Database Created Successfully ...\\source\\repos\\Company\\Company\\bin\\Debug");
                OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; Data Source=Master.mdb");
                con.Open();
                string strTemp = " [Id] Number, [FirstName] Text, [LastName] Text, [BirthDate] Text ";
                
                OleDbCommand com = new OleDbCommand();
                com.Connection = con;
                com.CommandText = "CREATE TABLE Employees(" + strTemp + ")";
                com.ExecuteNonQuery();
                com.Connection.Close();
                con.Close();
                cat = null;
                result = true;
            }catch(Exception ex)
            {
                result = false;
            }
            
            return result;
        }

    }
}
