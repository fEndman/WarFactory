package crc643f46942d9dd1fff9;


public abstract class NongreedySnapHelper
	extends androidx.recyclerview.widget.LinearSnapHelper
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_attachToRecyclerView:(Landroidx/recyclerview/widget/RecyclerView;)V:GetAttachToRecyclerView_Landroidx_recyclerview_widget_RecyclerView_Handler\n" +
			"";
		mono.android.Runtime.register ("Xamarin.Forms.Platform.Android.NongreedySnapHelper, Xamarin.Forms.Platform.Android", NongreedySnapHelper.class, __md_methods);
	}


	public NongreedySnapHelper ()
	{
		super ();
		if (getClass () == NongreedySnapHelper.class)
			mono.android.TypeManager.Activate ("Xamarin.Forms.Platform.Android.NongreedySnapHelper, Xamarin.Forms.Platform.Android", "", this, new java.lang.Object[] {  });
	}


	public void attachToRecyclerView (androidx.recyclerview.widget.RecyclerView p0)
	{
		n_attachToRecyclerView (p0);
	}

	private native void n_attachToRecyclerView (androidx.recyclerview.widget.RecyclerView p0);

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
