package crc643f46942d9dd1fff9;


public class PolylineView
	extends crc643f46942d9dd1fff9.ShapeView
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Xamarin.Forms.Platform.Android.PolylineView, Xamarin.Forms.Platform.Android", PolylineView.class, __md_methods);
	}


	public PolylineView (android.content.Context p0)
	{
		super (p0);
		if (getClass () == PolylineView.class)
			mono.android.TypeManager.Activate ("Xamarin.Forms.Platform.Android.PolylineView, Xamarin.Forms.Platform.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public PolylineView (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == PolylineView.class)
			mono.android.TypeManager.Activate ("Xamarin.Forms.Platform.Android.PolylineView, Xamarin.Forms.Platform.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public PolylineView (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == PolylineView.class)
			mono.android.TypeManager.Activate ("Xamarin.Forms.Platform.Android.PolylineView, Xamarin.Forms.Platform.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public PolylineView (android.content.Context p0, android.util.AttributeSet p1, int p2, int p3)
	{
		super (p0, p1, p2, p3);
		if (getClass () == PolylineView.class)
			mono.android.TypeManager.Activate ("Xamarin.Forms.Platform.Android.PolylineView, Xamarin.Forms.Platform.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2, p3 });
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
