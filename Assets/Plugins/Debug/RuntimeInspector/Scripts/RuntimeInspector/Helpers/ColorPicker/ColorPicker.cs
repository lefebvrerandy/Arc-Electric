using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace RuntimeInspectorNamespace
{
#pragma warning disable 649
	public class ColorPicker : SkinnedWindow
	{
		private static ColorPicker m_instance;
		public static ColorPicker Instance
		{
			get
			{
				if( m_instance == null )
				{
					//m_instance = Instantiate( Resources.Load<ColorPicker>( "RuntimeInspector/ColorPicker" ) );
					//m_instance.gameObject.SetActive( false );
				}

				return m_instance;
			}
		}
		public Light light; 

		private Color color;
		public Color GetColor
		{
			get
			{
				color.r = float.Parse(rInput.Text.ToString(), CultureInfo.InvariantCulture.NumberFormat);
				color.g = float.Parse(gInput.Text.ToString(), CultureInfo.InvariantCulture.NumberFormat);
				color.b = float.Parse(bInput.Text.ToString(), CultureInfo.InvariantCulture.NumberFormat);
				color.a = float.Parse(aInput.Text.ToString(), CultureInfo.InvariantCulture.NumberFormat);
				return color;
			}
		}
#pragma warning disable 0649
		[SerializeField]
		private Image panel;

		[SerializeField]
		private ColorWheelControl colorWheel;

		[SerializeField]
		private ColorPickerAlphaSlider alphaSlider;

		[SerializeField]
		private Text rgbaText;

		[SerializeField]
		private BoundInputField rInput;

		[SerializeField]
		private BoundInputField gInput;

		[SerializeField]
		private BoundInputField bInput;

		[SerializeField]
		private BoundInputField aInput;

		[SerializeField]
		private LayoutElement rgbaLayoutElement;

		[SerializeField]
		private LayoutElement buttonsLayoutElement;

		[SerializeField]
		private Button cancelButton;

		[SerializeField]
		private Button okButton;
#pragma warning restore 0649

		private Color initialValue;
		private ColorWheelControl.OnColorChangedDelegate onColorChanged;

		//protected override void Awake()
		//{
		//	base.Awake();

  //          rInput.Initialize();
		//	gInput.Initialize();
		//	bInput.Initialize();
		//	aInput.Initialize();

		//	cancelButton.onClick.AddListener( Cancel );
		//	okButton.onClick.AddListener( Close );
		//}

		//void Start()
		//{
		//	colorWheel.OnColorChanged += OnSelectedColorChanged;
		//	alphaSlider.OnValueChanged += OnAlphaChanged;

		//	rInput.DefaultEmptyValue = "0";
		//	gInput.DefaultEmptyValue = "0";
		//	bInput.DefaultEmptyValue = "0";
		//	aInput.DefaultEmptyValue = "0";

		//	rInput.Skin = Skin;
		//	gInput.Skin = Skin;
		//	bInput.Skin = Skin;
		//	aInput.Skin = Skin;

		//	rInput.OnValueChanged += OnRGBAChanged;
		//	gInput.OnValueChanged += OnRGBAChanged;
		//	bInput.OnValueChanged += OnRGBAChanged;
		//	aInput.OnValueChanged += OnRGBAChanged;

		//	OnSelectedColorChanged( colorWheel.Color );
		//}

		public void Show( ColorWheelControl.OnColorChangedDelegate onColorChanged, Color initialColor )
		{
			initialValue = initialColor;

			this.onColorChanged = null;
			colorWheel.PickColor( initialColor );
			alphaSlider.Color = initialColor;
			alphaSlider.Value = initialColor.a;
			this.onColorChanged = onColorChanged;

			( (RectTransform) panel.transform ).anchoredPosition = Vector2.zero;
			gameObject.SetActive( true );
		}

		public void Cancel()
		{
			if( colorWheel.Color != initialValue && onColorChanged != null )
				onColorChanged( initialValue );

			Close();
		}

		public void Close()
		{
			//Pass the rgba values to the associated game object
            if(light != null)
            {
                light.color = GetColor;
            }
	

			onColorChanged = null;	
			gameObject.SetActive( false );
		}

		protected override void RefreshSkin()
		{
			panel.color = Skin.WindowColor;

			rgbaLayoutElement.SetHeight( Skin.LineHeight );
			buttonsLayoutElement.SetHeight( Mathf.Min( 45f, Skin.LineHeight * 1.5f ) );

			rgbaText.SetSkinText( Skin );

			rInput.Skin = Skin;
			gInput.Skin = Skin;
			bInput.Skin = Skin;
			aInput.Skin = Skin;

			cancelButton.SetSkinButton( Skin );
			okButton.SetSkinButton( Skin );
		}

		private void OnSelectedColorChanged( Color32 color )
		{
			rInput.Text = "" + color.r;
			gInput.Text = "" + color.g;
			bInput.Text = "" + color.b;
			aInput.Text = "" + color.a;

			alphaSlider.Color = color;

			if( onColorChanged != null )
			{
				onColorChanged(color);
			}
		}

		private void OnAlphaChanged( float alpha )
		{
			aInput.Text = "" + (int) ( alpha * 255 );
			colorWheel.Alpha = alpha;

			Color color = colorWheel.Color;
			color.a = alpha;

			if( onColorChanged != null )
				onColorChanged( color );
		}

		private bool OnRGBAChanged( BoundInputField source, string input )
		{
			byte value;
			if( byte.TryParse( input, out value ) )
			{
				Color32 color = colorWheel.Color;
				if( source == rInput )
					color.r = value;
				else if( source == gInput )
					color.g = value;
				else if( source == bInput )
					color.b = value;
				else
				{
					color.a = value;
					alphaSlider.Value = value / 255f;
				}

				alphaSlider.Color = color;
				colorWheel.PickColor( color );
				return true;
			}

			return false;
		}
	}
#pragma warning restore 649
}