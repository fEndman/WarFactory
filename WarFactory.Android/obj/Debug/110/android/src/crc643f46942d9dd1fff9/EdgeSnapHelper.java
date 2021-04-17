package crc643f46942d9dd1fff9;


public abstract class EdgeSnapHelper
	extends crc643f46942d9dd1fff9.NongreedySnapHelper
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Xamarin.Forms.Platform.Android.EdgeSnapHelper, Xamarin.Forms.Platform.Android", EdgeSnapHelper.class, __md_methods);
	}


	public EdgeSnapHelper ()
	{
		super ();
		if (getClass () == EdgeSnapHelper.class)
			mono.android.TypeManager.Activate ("Xamarin.Forms.Platform.Android.EdgeSnapHelper, Xamarin.Forms.Platform.Android", "", this, new java.lang.Object[] {  });
	}

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
