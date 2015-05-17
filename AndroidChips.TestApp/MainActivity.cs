using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Com.Android.EX.Chips;
using Java.Lang;

namespace AndroidChips.TestApp
{
	[Activity(Label = "AndroidChips.TestApp", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		int count = 1;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.Main);

			var phoneRetv = FindViewById<RecipientEditTextView>(Resource.Id.phone_retv);
			phoneRetv.SetTokenizer(new MultiAutoCompleteTextView.CommaTokenizer());

			//var adapter = new BaseRecipientAdapter(BaseRecipientAdapter.QueryTypePhone, this) {ShowMobileOnly = true};
			var adapter = new MyAdapter2(this);

			phoneRetv.Adapter = adapter;
			phoneRetv.DismissDropDownOnItemSelected(true);

			new Handler().PostDelayed(() => { var chips = phoneRetv.GetSortedRecipients(); }, 5000);

			//var showAll = FindViewById<ImageButton>(Resource.Id.show_all);
			//showAll.Click += (sender, args) => { phoneRetv.ShowAllContacts(); };
		}
	}

	public class MyAdapter2 : BaseRecipientAdapter
	{
		public override unsafe Filter Filter
		{
			get
			{
				//return new DirectoryFilter(this, new DirectorySearchParams());
				return new MyFilter(this);
			}
		}

		protected MyAdapter2(IntPtr javaReference, JniHandleOwnership transfer)
			: base(javaReference, transfer)
		{
		}

		public MyAdapter2(Context p0, int p1, int p2)
			: base(p0, p1, p2)
		{
		}

		public MyAdapter2(int p0, Context p1, int p2)
			: base(p0, p1, p2)
		{
		}

		public MyAdapter2(int p0, Context p1)
			: base(p0, p1)
		{
		}

		public MyAdapter2(Context p0, int p1)
			: base(p0, p1)
		{
		}

		public MyAdapter2(Context p0)
			: base(p0)
		{
		}

		public void Foo(List<RecipientEntry> entries)
		{
			UpdateEntries(entries);
		}
	}

	public class MyFilter : Filter
	{
		private readonly BaseRecipientAdapter _baseRecipientAdapter;

		public MyFilter(BaseRecipientAdapter baseRecipientAdapter)
		{
			_baseRecipientAdapter = baseRecipientAdapter;
		}

		protected override FilterResults PerformFiltering(ICharSequence constraint)
		{
			var results = new FilterResults();
			var matchList = new List<string>();
			matchList.Add("test");

			Java.Lang.Object[] matchObjects;
			matchObjects = new Java.Lang.Object[matchList.Count];
			for (int i = 0; i < matchList.Count; i++)
			{
				matchObjects[i] = new Java.Lang.String(matchList[i]);
			}

			results.Values = matchObjects;
			results.Count = 1;
			return results;
		}

		protected override void PublishResults(ICharSequence constraint, FilterResults results)
		{
			var e = new List<RecipientEntry>();
			var r = RecipientEntry.ConstructFakeEntry("test", true);
			e.Add(r);

			((MyAdapter2)_baseRecipientAdapter).Foo(e);
		}
	}
}

