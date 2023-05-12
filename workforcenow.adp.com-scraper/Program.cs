
//https://workforcenow.adp.com/mascsr/default/mdf/recruitment/recruitment.html?cid=1e1c7010-328a-4fd5-8cab-54255cb16096&ccId=19000101_000001&type=JS&lang=en_US&jobId=467544
//https://workforcenow.adp.com/mascsr/default/mdf/recruitment/recruitment.html?cid=267cfc8f-e6d3-499b-8a38-c6abe8ee3d82&ccId=19000101_000001&type=JS&lang=en_US


using System.Net.Http.Json;

string cid = "77f1c5f3-df61-4649-affe-4ca5f56a0b5b";
int pages = 1;
List<JobModel> Jobs = new List<JobModel>();
try
{
    for (int i = 1; i <= pages; i++)
    {
        string url = $"https://workforcenow.adp.com/mascsr/default/careercenter/public/events/staffing/v1/job-requisitions?cid={cid}&timeStamp=1683837651824&lang=en_US&iccFlag=yes&eccFlag=yes&ccId=19000101_000001&locale=en_US&$skip={(20 * (i - 1)) + 1}&$top=20";
        HttpClient client = new HttpClient();
        var response = await client.GetFromJsonAsync<WokforceNow_ADP>(url);
        if (response != null)
        {
            if (i == 1)
            {
                Console.WriteLine("Total Jobs: "+ response.meta.totalNumber);
                pages = ((response.meta.totalNumber - 1) / 20) + 1;
                Console.WriteLine("Total Pages: "+pages);
            }

            foreach (var item in response.jobRequisitions)
            {
                var job = new JobModel
                {
                    Title = item.requisitionTitle,
                    Url = $"https://workforcenow.adp.com/mascsr/default/mdf/recruitment/recruitment.html?cid={cid}&ccId=19000101_000001&type=JS&lang=en_US&jobId={item.customFieldGroup.stringFields.FirstOrDefault(x => x.nameCode.codeValue == "ExternalJobID").stringValue}"
                };
                Jobs.Add(job);
                Console.WriteLine($"{job.Title} => {job.Url}");
            }
        }
    }
    Console.WriteLine("Total jobs extracted => " + Jobs.Count);
}
catch (Exception ex)
{
    Console.WriteLine("Unable to extract data from https://workforcenow.adp.com => cid="+cid);
}

Console.ReadKey();


public class JobModel
{
    public string Url { get; set; }
    public string Title { get; set; }
}

public class WokforceNow_ADP
{
    public List<JobRequisition> jobRequisitions { get; set; }
    public ADPMeta meta { get; set; }
}

public class JobRequisition
{
   
    public string requisitionTitle { get; set; }
    
    public CustomFieldGroup customFieldGroup { get; set; }
}

public class ADPMeta
{
    public int totalNumber { get; set; }
}

public class CustomFieldGroup
{
    public List<StringField> stringFields { get; set; }
}

public class StringField
{
    public string stringValue { get; set; }
    public NameCode nameCode { get; set; }
}

public class NameCode
{
    public string codeValue { get; set; }
    public string shortName { get; set; }
}
