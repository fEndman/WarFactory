package crc643f46942d9dd1fff9;


public class ScrollLayoutManager
	extends androidx.recyclerview.widget.LinearLayoutManager
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_canScrollVertically:()Z:GetCanScrollVerticallyHandler\n" +
			"";
		mono.android.Runtime.register ("Xamarin.Forms.Platform.Android.ScrollLayoutManager, Xamarin.Forms.Platform.Android", ScrollLayoutManager.class, __md_methods);
	}


	public ScrollLayoutManager (android.content.Context p0)
	{
		super (p0);
		if (getClass () == ScrollLayoutManager.class)
			mono.android.TypeManager.Activate ("Xamarin.Forms.Platform.Android.ScrollLayoutManager, Xamarin.Forms.Platform.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public ScrollLayoutManager (android.content.Context p0, android.util.AttributeSet p1, int p2, int p3)
	{
		super (p0, p1, p2, p3);
		if (getClass () == ScrollLayoutManager.class)
			mono.android.TypeManager.Activate ("Xamarin.Forms.Platform.Android.ScrollLayoutManager, Xamarin.Forms.Platform.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}


	public ScrollLayoutManager (android.content.Context p0, int p1, boolean p2)
	{
		super (p0, p1, p2);
		if (getClass () == ScrollLayoutManager.class)
			mono.android.TypeManager.Activate ("Xamarin.Forms.Platform.Android.ScrollLayoutManager, Xamarin.Forms.Platform.Android", "Android.Content.Context, Mono.Android:System.Int32, mscorlib:System.Boolean, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public boolean canScrollVertically ()
	{
		return n_canScrollVertically ();
	}

	private native boolean n_canScrollVertically ();

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
