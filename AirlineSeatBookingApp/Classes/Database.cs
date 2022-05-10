using System.Text;

namespace AirlineSeatBookingApp.Classes
{
    public class Database
    {
        List<Models.PassengerModel> passengerList = new List<Models.PassengerModel>();
        string fileUrl = @".\Data\Passengers.txt";

        public List<Models.PassengerModel> ReadPassengerList()
        {
            int count = 1;

            passengerList.Clear();

            //Read passengers from file (comma delimited "Id,Name")
            string[] passengers = File.ReadAllLines(fileUrl);

            //Loop through each passenger line
            foreach (string passenger in passengers)
            {
                passengerList.Add(new Models.PassengerModel { Id = count, Name = passenger });
                count++;
            }

            return passengerList;
        }

        public List<Models.PassengerModel> AddPassenger(string name)
        {
            string formattedName = name;

            var tupleReturn = formatName(formattedName);

            if (tupleReturn.Item1.Count > 0)
            {
                return ReadPassengerList();
            }

            string[] passengerList = File.ReadAllLines(fileUrl);

            string[] passengerListCopy = new string[passengerList.Count() + 1];

            for (int i = 0; i < passengerList.Length; i++)
            {
                passengerListCopy[i] = passengerList[i];
            }

            passengerListCopy[passengerList.Count()] = tupleReturn.Item2;

            writeToDatabase(passengerListCopy);

            return ReadPassengerList();
        }
        public List<Models.PassengerModel> DeletePassenger(string name)
        {
            int count = 0;

            name = name is null ? "" : name;

            string[] passengerList = File.ReadAllLines(fileUrl);

            string[] passengerListCopy = new string[passengerList.Count() -1];

 
            foreach (var passenger in passengerList)
            {
                if (passenger != name)
                {
                    passengerListCopy[count] = passenger;
                    count++;
                }
            }

            writeToDatabase(passengerListCopy);

            return ReadPassengerList();
        }

        private void writeToDatabase(string[] passengerList)
        {
            var file = File.Create(fileUrl);
            file.Close();
            File.AppendAllLines(fileUrl, passengerList);
        }

        private (List<string>, string) formatName(string name)
        {
            List<string> errorList = new List<string>();
            string formattedName = name;
            //string tempString;

            if (name != null)
            { 
                name = name.Trim();
            //Check name for alpha characters, spaces or hyphen or '
            byte[] array = Encoding.ASCII.GetBytes(name);

            for (var i=0; i < array.Length; i++)
            {
                if (!((array[i] >= 65 && array[i] <= 90) || (array[i] >= 97 && array[i] <= 122) ||
                    array[i] == 32 || array[i] == 45 || array[i] == 39 || array[i] == 96)) {
                    //return ("Invalid character '" + name[i] + "'  in name.",name);
                    errorList.Add("Invalid character '" + name[i] + "'  in name.");
                }
            }

            //Name must contain more than a single word.
            if (!name.Contains(' '))
            {
                //return ("Name must have firstname and surname.", name);
                errorList.Add("Name must have firstname and surname.");
            }

            //Default everything to lowercase.
            formattedName = name.ToLower();

            //Check that the first letter and last letter is not a hyphen or '. 
            //If so, error, otherwise capitalise it
            if (!((array[0] >= 65 && array[0] <= 90) || (array[0] >= 97 && array[0] <= 122))
                && ((array[array.Length - 1] >= 65 && array[array.Length - 1] <= 90) || (array[array.Length - 1] >= 97 && array[array.Length - 1] <= 122)))
            {
                //return ("Firstname must start and surname must end with a character.", name);
                errorList.Add("Firstname must start and surname must end with a character.");
            }

            formattedName = ((char)(array[0] - 32)) + formattedName.Substring(1);
            
            //First letter of each word needs to be capialised.
            var pos = formattedName.IndexOf(' ');

            while (pos > 0)
            {
                if (!((array[pos + 1] >= 65 && array[pos + 1] <= 90) || (array[pos + 1] >= 97 && array[pos + 1] <= 122)))
                {
                    errorList.Add("First letter of each middlename/surname must start with a character.");
                }

                if (!((array[pos - 1] >= 65 && array[pos - 1] <= 90) || (array[pos-1] >= 97 && array[pos-1] <= 122)))
                {
                    errorList.Add("Last letter of each firstname/middlename must end with a character.");
                }

                formattedName = formattedName.Substring(0, pos + 1) + ((char)(array[pos +1] - 32)) + formattedName.Substring(pos + 2);
                pos = formattedName.IndexOf(' ', pos + 1);
            }

            //Capitalise after the hyphen.
            pos = formattedName.IndexOf('-');

            while (pos > 0)
            {
                formattedName = formattedName.Substring(0, pos + 1) + ((char)(array[pos + 1] - 32)) + formattedName.Substring(pos + 2);
                pos = formattedName.IndexOf('-', pos + 1);
            }

            //Capitalise after the '.
            pos = formattedName.IndexOf("'");

            while (pos > 0)
            {
                formattedName = formattedName.Substring(0, pos + 1) + ((char)(array[pos + 1] - 32)) + formattedName.Substring(pos + 2);
                pos = formattedName.IndexOf("'", pos + 1);
            }
            }

            return (errorList, formattedName);

        }
    }

}
