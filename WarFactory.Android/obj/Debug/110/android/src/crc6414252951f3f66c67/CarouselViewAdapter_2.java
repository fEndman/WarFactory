package crc6414252951f3f66c67;


public class CarouselViewAdapter_2
	extends crc643f46942d9dd1fff9.ItemsViewAdapter_2
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_getItemCount:()I:GetGetItemCountHandler\n" +
			"n_onBindViewHolder:(Landroidx/recyclerview/widget/RecyclerView$ViewHolder;I)V:GetOnBindViewHolder_Landroidx_recyclerview_widget_RecyclerView_ViewHolder_IHandler\n" +
			"";
		mono.android.Runtime.register ("Xamarin.Forms.Platform.Android.CollectionView.CarouselViewAdapter`2, Xamarin.Forms.Platform.Android", CarouselViewAdapter_2.class, __md_methods);
	}


	public CarouselViewAdapter_2 ()
	{
		super ();
		if (getClass () == CarouselViewAdapter_2.class)
			mono.android.TypeManager.Activate ("Xamarin.Forms.Platform.Android.CollectionView.CarouselViewAdapter`2, Xamarin.Forms.Platform.Android", "", this, new java.lang.Object[] {  });
	}


	public int getItemCount ()
	{
		return n_getItemCount ();
	}

	private native int n_getItemCount ();


	public void onBindViewHolder (androidx.recyclerview.widget.RecyclerView.ViewHolder p0, int p1)
	{
		n_onBindViewHolder (p0, p1);
	}

	private native void n_onBindViewHolder (androidx.recyclerview.widget.RecyclerView.ViewHolder p0, int p1);

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
