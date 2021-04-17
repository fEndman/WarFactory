package crc643f46942d9dd1fff9;


public class GridLayoutSpanSizeLookup
	extends androidx.recyclerview.widget.GridLayoutManager.SpanSizeLookup
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_getSpanSize:(I)I:GetGetSpanSize_IHandler\n" +
			"";
		mono.android.Runtime.register ("Xamarin.Forms.Platform.Android.GridLayoutSpanSizeLookup, Xamarin.Forms.Platform.Android", GridLayoutSpanSizeLookup.class, __md_methods);
	}


	public GridLayoutSpanSizeLookup ()
	{
		super ();
		if (getClass () == GridLayoutSpanSizeLookup.class)
			mono.android.TypeManager.Activate ("Xamarin.Forms.Platform.Android.GridLayoutSpanSizeLookup, Xamarin.Forms.Platform.Android", "", this, new java.lang.Object[] {  });
	}


	public int getSpanSize (int p0)
	{
		return n_getSpanSize (p0);
	}

	private native int n_getSpanSize (int p0);

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
