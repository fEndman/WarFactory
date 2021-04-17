package crc643f46942d9dd1fff9;


public class GradientStrokeDrawable_GradientShaderFactory
	extends android.graphics.drawable.ShapeDrawable.ShaderFactory
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_resize:(II)Landroid/graphics/Shader;:GetResize_IIHandler\n" +
			"";
		mono.android.Runtime.register ("Xamarin.Forms.Platform.Android.GradientStrokeDrawable+GradientShaderFactory, Xamarin.Forms.Platform.Android", GradientStrokeDrawable_GradientShaderFactory.class, __md_methods);
	}


	public GradientStrokeDrawable_GradientShaderFactory ()
	{
		super ();
		if (getClass () == GradientStrokeDrawable_GradientShaderFactory.class)
			mono.android.TypeManager.Activate ("Xamarin.Forms.Platform.Android.GradientStrokeDrawable+GradientShaderFactory, Xamarin.Forms.Platform.Android", "", this, new java.lang.Object[] {  });
	}


	public android.graphics.Shader resize (int p0, int p1)
	{
		return n_resize (p0, p1);
	}

	private native android.graphics.Shader n_resize (int p0, int p1);

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
