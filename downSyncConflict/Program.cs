using downSyncConflict;

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
            Name = i == 2 ? $"heee" : $"Job Name{i}",
            Status = GetRandomStatus(1),
            Description = $"Job Description{i}",
            Budget = 100 * i,
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


void Solution1()
{

    var ejobs = GetSaveJobsDB();

    var xpmjobs = GetXPMJobsFromAPI();

    var conflictJobs = new List<JobConflict>();

    foreach (var xpmjob in xpmjobs)
    {
        List<ChangedProperty> changeProps = new();

        var job = ejobs.Where(x => x.Id == xpmjob.Id).FirstOrDefault();

        if (job == null)
        {
            continue;
        }

        if (!xpmjob.Name.Equals(job.Name))
        {
            changeProps.Add(new ChangedProperty
            {
                FieldName = nameof(BaseJob.Name),
                OldValue = job.Name,
                NewValue = xpmjob.Name
            });
        }

        if (!xpmjob.Status.Equals(job.Status))
        {
            changeProps.Add(new ChangedProperty
            {
                FieldName = nameof(BaseJob.Status),
                OldValue = job.Status,
                NewValue = xpmjob.Status
            });
        }

        if (!xpmjob.Description.Equals(job.Description))
        {
            changeProps.Add(new ChangedProperty
            {
                FieldName = nameof(BaseJob.Description),
                OldValue = job.Description,
                NewValue = xpmjob.Description
            });
        }

        if (xpmjob.Budget != job.Budget)
        {
            changeProps.Add(new ChangedProperty
            {
                FieldName = nameof(BaseJob.Budget),
                OldValue = job.Budget.ToString(),
                NewValue = xpmjob.Budget.ToString()
            });
        }


        if (changeProps.Any())
        {
            conflictJobs.Add(new JobConflict
            {
                Id = xpmjob.Id,
                Name = xpmjob.Name,
                Status = xpmjob.Status,
                Description = xpmjob.Description,
                Budget = xpmjob.Budget,
                ChangedProperties = changeProps,
            });
        }

    }

    foreach (var conflictJob in conflictJobs)
    {

        Console.WriteLine(conflictJob);
        Console.WriteLine("-------");

    }
}


void Solution2()
{
    var ejobs = GetSaveJobsDB();

    var xpmjobs = GetXPMJobsFromAPI();

    var conflictJobs = new List<JobConflict>();


    var jobComparer = new JobComparer<BaseJob>();

    foreach (var xpmjob in xpmjobs)
    {
        var job = ejobs.FirstOrDefault(x => x.Id == xpmjob.Id);

        if (job != null)
        {
            var changeProps = jobComparer.Compare(job, xpmjob);

            if (changeProps.Any())
            {
                conflictJobs.Add(new JobConflict
                {
                    Id = xpmjob.Id,
                    Name = xpmjob.Name,
                    Status = xpmjob.Status,
                    Description = xpmjob.Description,
                    Budget = xpmjob.Budget,
                    ChangedProperties = changeProps,
                });
            }
        }
    }

    foreach (var conflictJob in conflictJobs)
    {

        Console.WriteLine(conflictJob);
        Console.WriteLine("-------");

    }
}

Solution2();