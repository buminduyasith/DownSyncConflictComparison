using downSyncConflict;
using static System.Reflection.Metadata.BlobBuilder;

List<EJob> GetSaveJobsDB()
{
    List<EJob> dummyData = new List<EJob>();

    for (int i = 1; i <= 10; i++)
    {
        EJob job = new EJob
        {
            Id = i,
            Name = $"Job Name{i}",
            Status = GetRandomStatus(),
            Description = $"Job Description{i}",
            Budget = 100 * i,

        };

        dummyData.Add(job);
    }

    return dummyData;
}


List<XPMJob> GetXPMJobsFromAPI()
{
    List<XPMJob> dummyData = new List<XPMJob>();

    for (int i = 1; i <= 10; i++)
    {
        XPMJob job = new XPMJob
        {
            Id = i,
            Title = i == 2 ? $"heee" : $"Job Name{i}",
            State = GetRandomStatus(1),
            JobDescription = $"Job Description{i}",
            JobBudget = i == 3 ? 100 * i * 2 : 100 * i
        };

        dummyData.Add(job);
    }

    return dummyData;
}

string GetRandomStatus(int index=0)
{
    string[] statuses = { "Pending", "InProgress", "Completed", "Failed" };
    Random random = new Random();
    int indexx = random.Next(statuses.Length);
    return statuses[index];
}


void Solution3()
{
    var ejobs = GetSaveJobsDB();

    var xpmjobs = GetXPMJobsFromAPI();

    var conflictJobs = new List<JobConflict>();


    List<PropertyMapping> propertyMappings = new List<PropertyMapping>
    {
        new PropertyMapping { ProviderPropertyName = nameof(XPMJob.State), WXFPropertyName =  nameof(EJob.Status), CustomFieldName = "Job Status" },       // Map "State" in XPMJob to "Status" in EJob
        new PropertyMapping { ProviderPropertyName = nameof(XPMJob.JobBudget), WXFPropertyName =  nameof(EJob.Budget) },  
        new PropertyMapping { ProviderPropertyName =  nameof(XPMJob.Title),  WXFPropertyName =  nameof(EJob.Name), CustomFieldName = "Job Name" }
    };

    var jobComparer = new PropertyComparer<XPMJob, EJob>(propertyMappings);


    foreach (var xpmjob in xpmjobs)
    {
        var job = ejobs.FirstOrDefault(x => x.Id == xpmjob.Id);

        if (job == null)
        {
            continue;
        }

        var changeProps = jobComparer.Compare(xpmjob, job);

        if (!changeProps.Any())
        {
            continue;
        }

        conflictJobs.Add(new JobConflict
        {
            Id = xpmjob.Id,
            Name = xpmjob.Title,
            Status = xpmjob.State,
            Description = xpmjob.JobDescription,
            Budget = xpmjob.JobBudget,
            ChangedProperties = changeProps,
        });
    }

    foreach (var conflictJob in conflictJobs)
    {

        Console.WriteLine(conflictJob);
        Console.WriteLine("-------");

    }
}

Solution3();


