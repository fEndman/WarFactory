package mono.com.google.android.material.bottomnavigation;


public class BottomNavigationView_OnNavigationItemReselectedListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.google.android.material.bottomnavigation.BottomNavigationView.OnNavigationItemReselectedListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onNavigationItemReselected:(Landroid/view/MenuItem;)V:GetOnNavigationItemReselected_Landroid_view_MenuItem_Handler:Google.Android.Material.BottomNavigation.BottomNavigationView/IOnNavigationItemReselectedListenerInvoker, Xamarin.Google.Android.Material\n" +
			"";
		mono.android.Runtime.register ("Google.Android.Material.BottomNavigation.BottomNavigationView+IOnNavigationItemReselectedListenerImplementor, Xamarin.Google.Android.Material", BottomNavigationView_OnNavigationItemReselectedListenerImplementor.class, __md_methods);
	}


	public BottomNavigationView_OnNavigationItemReselectedListenerImplementor ()
	{
		super ();
		if (getClass () == BottomNavigationView_OnNavigationItemReselectedListenerImplementor.class)
			mono.android.TypeManager.Activate ("Google.Android.Material.BottomNavigation.BottomNavigationView+IOnNavigationItemReselectedListenerImplementor, Xamarin.Google.Android.Material", "", this, new java.lang.Object[] {  });
	}


	public void onNavigationItemReselected (android.view.MenuItem p0)
	{
		n_onNavigationItemReselected (p0);
	}

	private native void n_onNavigationItemReselected (android.view.MenuItem p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
