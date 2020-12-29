using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkflowManager.EFCoreLibrary.DataAccess;
using WorkflowManager.EFCoreLibrary.Entities;

namespace WorkflowManager.WebUI.Helpers
{
	public static class ScheduleCalculations
	{
		private static readonly WorkflowManagerRepository _repository = new WorkflowManagerRepository();
		private static readonly int workStartHour = 7;
		private static readonly int workStartMinute = 0;
		private static readonly int workEndHour = 15;
		private static readonly int workEndMinute = 0;

		public static void CalcBuilidngWorkSchedule(int buildingId)
		{
			
			Building Building = _repository.BuildingRepository.SearchFor(b => b.Id == buildingId)
				.Include(b => b.Jobs)
					.ThenInclude(j => j.UserJobs)
						.ThenInclude(uj => uj.User)
				.FirstOrDefault()
			;
			List<Job> aggregated = new List<Job>();
			List<Job> toAggregate = Building.Jobs.Where(j => j.Done != true).ToList();
			int numOfItems = toAggregate.Count();
			for (int i = 0; i < numOfItems; i++)
			{
				Job job = toAggregate.Aggregate((j1, j2) => j1.Order < j2.Order ? j1 : j2);
				_repository._context.Entry(job).Reload();
				toAggregate.Remove(job);
				if (job.UserJobs.Any())
				{
					IEnumerable<Job> prevJobs = aggregated.Where(j => j.UserJobs.Select(uj => uj.UserId).Intersect(job.UserJobs.Select(uj => uj.UserId)).Any());
					if (prevJobs.Any())
					{
						Job prevJob = GetPredictedLastJobsThatUsersWillDo(prevJobs).Aggregate((j1, j2) => j1.PredictedDoneDate < j2.PredictedDoneDate ? j1 : j2);
						job.PredictedDoneDate = AddWorkTime(prevJob.PredictedDoneDate, job.PredictedDuration);
					}
					else
					{
						IEnumerable<Job> doneJobs = Building.Jobs.Where(j => j.Done == true && j.UserJobs.Select(uj => uj.UserId).Intersect(job.UserJobs.Select(uj => uj.UserId)).Any());
						if (doneJobs.Any())
						{
							Job prevDoneJob = GetLastJobsThatUsersDone(doneJobs).Aggregate((j1, j2) => j1.DoneDate < j2.DoneDate ? j1 : j2);
							job.PredictedDoneDate = AddWorkTime(prevDoneJob.DoneDate, job.PredictedDuration);
						}
						else
							job.PredictedDoneDate = AddWorkTime(job.DateAdded, job.PredictedDuration);
					}
				}
				else
				{
					IEnumerable<Job> prevJobs = aggregated.Where(j => j.Order < job.Order);
					if (prevJobs.Any())
					{
						Job prevJob = prevJobs.Aggregate((j1, j2) => j1.Order > j2.Order ? j1 : j2);
						job.PredictedDoneDate = AddWorkTime(prevJob.PredictedDoneDate, job.PredictedDuration);
					}
					else
					{
						IEnumerable<Job> prevDoneJobs = Building.Jobs.Where(j => j.Done);

						if (prevDoneJobs.Any())
						{
							Job prevJob = prevDoneJobs.Aggregate((j1, j2) => j1.DoneDate > j2.DoneDate ? j1 : j2);
							job.PredictedDoneDate = AddWorkTime(prevJob.DoneDate, job.PredictedDuration);
						}
						else
							job.PredictedDoneDate = AddWorkTime(job.DateAdded, job.PredictedDuration);
					}
				}
				aggregated.Add(job);
			}

			foreach (Job job in aggregated)
			{
				_repository.JobRepository.Update(job);
			}
			_repository.SaveChanges();
		}
		public static IEnumerable<Job> GetLastJobsThatUsersDone(IEnumerable<Job> sourceJobs)
		{
			List<User> targetUsers = new List<User>();
			List<Job> lastUsersJobs = new List<Job>();

			foreach (Job job in sourceJobs)
			{
				IEnumerable<User> currJobUsers = job.UserJobs.Select(uj => uj.User);
				foreach (User user in currJobUsers)
				{
					if (!targetUsers.Contains(user))
						targetUsers.Add(user);
				}
			}
			foreach (User user in targetUsers)
			{
				IEnumerable<Job> jobs = sourceJobs.Where(j => j.UserJobs.Any(uj => uj.UserId == user.Id));
				lastUsersJobs.Add(jobs.Aggregate((j1, j2) => j1.DoneDate > j2.DoneDate ? j1 : j2));
			}
			return lastUsersJobs;
		}

		public static IEnumerable<Job> GetPredictedLastJobsThatUsersWillDo(IEnumerable<Job> sourceJobs)
		{
			List<User> targetUsers = new List<User>();
			List<Job> lastUsersJobs = new List<Job>();

			foreach (Job job in sourceJobs)
			{
				IEnumerable<User> currJobUsers = job.UserJobs.Select(uj => uj.User);
				foreach (User user in currJobUsers)
				{
					if (!targetUsers.Contains(user))
						targetUsers.Add(user);
				}
			}
			foreach (User user in targetUsers)
			{
				IEnumerable<Job> jobs = sourceJobs.Where(j => j.UserJobs.Any(uj => uj.UserId == user.Id));
				lastUsersJobs.Add(jobs.Aggregate((j1, j2) => j1.PredictedDoneDate > j2.PredictedDoneDate ? j1 : j2));
			}
			return lastUsersJobs;
		}




		public static async Task<Job> CalcNewAddedJobPredictedDoneDate(Job target)
		{
			IEnumerable<Job> buildingJobs = _repository.JobRepository.SearchFor(j => j.IdBuilding == target.IdBuilding);

			if (target.UserJobs.Any())
			{
				IEnumerable<Job> prevJobs = buildingJobs.Where(j => !j.Done && j.UserJobs.Select(uj => uj.UserId).Intersect(target.UserJobs.Select(uj => uj.UserId)).Any());
				if (prevJobs.Any())
				{
					Job prevJob = GetPredictedLastJobsThatUsersWillDo(prevJobs).Aggregate((j1, j2) => j1.PredictedDoneDate < j2.PredictedDoneDate ? j1 : j2);
					target.PredictedDoneDate = AddWorkTime(prevJob.PredictedDoneDate, target.PredictedDuration);
				}
				else
				{
					IEnumerable<Job> doneJobs = buildingJobs.Where(j => j.Done && j.UserJobs.Select(uj => uj.UserId).Intersect(target.UserJobs.Select(uj => uj.UserId)).Any());
					if (doneJobs.Any())
					{
						Job prevDoneJob = GetLastJobsThatUsersDone(doneJobs).Aggregate((j1, j2) => j1.DoneDate < j2.DoneDate ? j1 : j2);
						target.PredictedDoneDate = AddWorkTime(prevDoneJob.DoneDate, target.PredictedDuration);
					}
					else
						target.PredictedDoneDate = AddWorkTime(target.DateAdded, target.PredictedDuration);
				}
			}
			else
			{
				IEnumerable<Job> prevJobs = buildingJobs.Where(j => !j.Done);
				if (prevJobs.Any())
				{
					Job prevJob = prevJobs.Aggregate((j1, j2) => j1.Order > j2.Order ? j1 : j2);
					target.PredictedDoneDate = AddWorkTime(prevJob.PredictedDoneDate, target.PredictedDuration);
				}
				else
				{
					IEnumerable<Job> prevDoneJobs = buildingJobs.Where(j => j.Done);

					if (prevDoneJobs.Any())
					{
						Job prevJob = prevDoneJobs.Aggregate((j1, j2) => j1.DoneDate > j2.DoneDate ? j1 : j2);
						target.PredictedDoneDate = AddWorkTime(prevJob.DoneDate, target.PredictedDuration);
					}
					else
						target.PredictedDoneDate = AddWorkTime(target.DateAdded, target.PredictedDuration);
				}
			}
			return target;
		}

		public static Job[] SortJobsBySchedule(IEnumerable<Job> source)
		{
			Job[] result = new Job[source.Count()];
			List<Job> doneJobs = source.Where(j => j.Done).ToList();
			List<Job> currJobs = source.Where(j => !j.Done).ToList();
			int j = 0;
			int doneJobsCount = doneJobs.Count();
			int currJobsCount = currJobs.Count();
			for (int i = 0; i < doneJobsCount; i++)
			{
				Job job = doneJobs.Aggregate((j1, j2) => j1.DoneDate < j2.DoneDate ? j1 : j2);
				doneJobs.Remove(job);
				result[j] = job;
				j++;
			}
			for (int i = 0; i < currJobsCount; i++)
			{
				Job job = currJobs.Aggregate((j1, j2) => j1.Order < j2.Order ? j1 : j2);
				currJobs.Remove(job);

				result[j] = job;
				j++;
			}

			return result;
		}

		public static DateTime AddWorkTime(DateTime date, TimeSpan workAmount)
		{
			int minutesToAdd = (int)workAmount.TotalMinutes;
			int workMinutes = (workEndHour * 60 + workEndMinute) - (workStartHour * 60 + workStartMinute);

			if (date.Hour * 60 + date.Minute < workEndHour * 60 + workEndMinute && date.Hour * 60 + date.Minute > workStartHour * 60 + workStartMinute)
			{
				int firstDayMinutesLeft = (workEndHour * 60 + workEndMinute) - (date.Hour * 60 + date.Minute);
				if (minutesToAdd > firstDayMinutesLeft)
				{
					minutesToAdd -= firstDayMinutesLeft;
					date = NextWorkDay(date);
				}
				else
				{
					return date.AddMinutes(minutesToAdd);
				}
			}
			else if (date.Hour * 60 + date.Minute < workStartHour * 60 + workStartMinute )
				date = DateTime.Parse(date.ToString("yyyy-MM-dd") + " " + workStartHour.ToString() + ":" + workStartMinute.ToString());
			else if(date.Hour * 60 + date.Minute > workEndHour * 60 + workEndMinute)
			{
				date = NextWorkDay(date);
				date = DateTime.Parse(date.ToString("yyyy-MM-dd") + " " + workStartHour.ToString() + ":" + workStartMinute.ToString());
			}


			while(minutesToAdd > workMinutes)
			{
				date = NextWorkDay(date);
				minutesToAdd -= workMinutes;
			}


			return date.AddMinutes(minutesToAdd);
		}
		public static DateTime NextWorkDay(DateTime startDate)
		{
			startDate.AddDays(1);
			if (startDate.DayOfWeek == DayOfWeek.Saturday)
				startDate.AddDays(2);
			if (startDate.DayOfWeek == DayOfWeek.Sunday)
				startDate.AddDays(1);

			return DateTime.Parse(string.Format("{0} {1}:{2}", startDate.ToString("yyyy-MM-dd"), workStartHour.ToString(), workStartMinute.ToString()));
		}	}
}
