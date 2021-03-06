﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using System.IO;

namespace TfsProjectListing
{
	class Program
	{
		public static string TFSServer = "http://fcctfs.cccs.co.uk:8080/tfs/basic"; // remember to add port number to server :8080/tfs/
		public static string FileOutputPath = @"";
		static void Main(string[] args)
		{
			string serverName = TFSServer;

			// Construct the server object
			TfsTeamProjectCollection tfs = new TfsTeamProjectCollection(new Uri(serverName));
			VersionControlServer vcs = tfs.GetService<VersionControlServer>();

			// Query root branches
			BranchObject[] bos = vcs.QueryRootBranchObjects(RecursionType.Full);

			// Can be used to display each root branch
			//Array.ForEach(bos, (bo) => DisplayAllBranches(bo, vcs));

			// 4. Write to file
			FileStream ostrm;
			StreamWriter writer;
			TextWriter oldOut = Console.Out;
			try
			{
				ostrm = new FileStream(string.Format("{0}TFSListing.txt", FileOutputPath), FileMode.OpenOrCreate, FileAccess.Write);
				writer = new StreamWriter(ostrm);
			}
			catch (Exception e)
			{
				Console.WriteLine("Cannot open TFSListing.txt for writing");
				Console.WriteLine(e.Message);
				return;
			}
			Console.SetOut(writer);
			
			// Grab each item name where not deleted
			foreach (var item in bos.OrderBy(bo => bo.Properties.RootItem.Item))
			{
				// ensure item is not already marked as deleted
				if (!item.Properties.RootItem.IsDeleted)
					Console.WriteLine(item.Properties.RootItem.Item);
			}

			Console.SetOut(oldOut);
			writer.Close();
			ostrm.Close();
		}

		///// <summary>
		///// Queries all branches against branch object
		///// </summary>
		///// <param name="bo"></param>
		///// <param name="vcs"></param>
		//private static void DisplayAllBranches(BranchObject bo, VersionControlServer vcs)
		//{
		//	//0.Prepare display indentation
		//	for (int tabcounter = 0; tabcounter < 1; tabcounter++)
		//		Console.Write("\t");

		//	//1.Display the current branch
		//	Console.WriteLine(string.Format("{0}", bo.Properties.RootItem.Item));

		//	//2.Query all child branches (one level deep)
		//	BranchObject[] childBos = vcs.QueryBranchObjects(bo.Properties.RootItem, RecursionType.OneLevel);

		//	//2.1 Order branch objects

		//	//3.Display all children recursively
		//	//recursionlevel++;
		//	foreach (BranchObject child in childBos)
		//	{
		//		if (child.Properties.RootItem.Item == bo.Properties.RootItem.Item)
		//			continue;

		//		DisplayAllBranches(child, vcs);
		//	}
		//	//recursionlevel--;
		//}

		//private static int recursionlevel = 0;
	}

}
