namespace Laba_2
{
    public partial class MainWindow
    {
        public class Danger
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Sourse { get; set; }
            public string Target { get; set; }
            public string Confidentiality { get; set; }
            public string Integrity { get; set; }
            public string Access { get; set; }
            public string DataCreate { get; set; }
            public string DataChange { get; set; }

            public Danger(string id, string name)
            {
                Id = "УБИ."+id;
                Name = name;
            }

            public Danger(string id, string name, string description, string sourse, string target, string confidentiality, string integrity, string access, string dataCreate, string dataChange)
            {
                Id = id;
                Name = name;
                Description = description;
                Sourse = sourse;
                Target = target;
                if (confidentiality=="1")
                {
                    Confidentiality = "Да";
                }
                else
                {
                    Confidentiality = "Нет";
                }
                if (integrity == "1")
                {
                    Integrity = "Да";
                }
                else
                {
                    Integrity = "Нет";
                }
                if (confidentiality=="1")
                {
                    Access = "Да";
                }
                else
                {
                    Access = "Нет";
                }         
                                
                DataCreate = dataCreate;
                DataChange = dataChange;
            }
        }


    }
}
