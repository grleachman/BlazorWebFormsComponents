using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;
using System.Web;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing.Design;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.Runtime.Serialization;

namespace BlazorWebFormsComponents
{
	public sealed class StateItem
	{
		private object value;
		private bool isDirty;

		/*
		 * Constructs a StateItem with an initial value.
		 */
		internal StateItem(object initialValue)
		{
			value = initialValue;
			isDirty = false;
		}

		/*
		 * Property to indicate StateItem has been modified.
		 */

		/// <devdoc>
		/// <para>Indicates whether the <see cref='System.Web.UI.StateItem'/> object has been modified.</para>
		/// </devdoc>
		public bool IsDirty
		{
			get
			{
				return isDirty;
			}
			set
			{
				isDirty = value;
			}
		}

		/*
		 * Property to access the StateItem value.
		 */

		/// <devdoc>
		/// <para>Indicates the value of the item that is stored in the <see cref='System.Web.UI.StateBag'/> 
		/// object.</para>
		/// </devdoc>
		public object Value
		{
			get
			{
				return value;
			}
			set
			{
				this.value = value;
			}
		}
	}

	internal static class SR
	{
		internal static string Parameter_Invalid = "Parameter_Invalid";
		internal static string Parameter_NullOrEmpty = "parameter_NullOrEmpty";
		internal static string Property_Invalid = "Property_Invalid";
		internal static string Property_NullOrEmpty = "Property_NullOrEmpty";
		internal static string Unexpected_Error = "Unexpected_Error";

		internal static string GetString(string message, string parameter)
		=> $"{message}, {parameter}";
	}
	static internal class ExceptionUtil
	{
		static internal ArgumentException ParameterInvalid(string parameter)
		{
			return new ArgumentException(SR.GetString(SR.Parameter_Invalid, parameter), parameter);
		}

		static internal ArgumentException ParameterNullOrEmpty(string parameter)
		{
			return new ArgumentException(SR.GetString(SR.Parameter_NullOrEmpty, parameter), parameter);
		}

		static internal ArgumentException PropertyInvalid(string property)
		{
			return new ArgumentException(SR.GetString(SR.Property_Invalid, property), property);
		}

		static internal ArgumentException PropertyNullOrEmpty(string property)
		{
			return new ArgumentException(SR.GetString(SR.Property_NullOrEmpty, property), property);
		}

		static internal InvalidOperationException UnexpectedError(string methodName)
		{
			return new InvalidOperationException(SR.GetString(SR.Unexpected_Error, methodName));
		}
	}

	public sealed class StateBag : IStateManager, IDictionary
	{
		private IDictionary bag;
		private bool marked;
		private bool ignoreCase;

		/*
		 * Constructs an StateBag
		 */

		/// <devdoc>
		/// <para>Initializes a new instance of the <see cref='System.Web.UI.StateBag'/> class.</para>
		/// </devdoc>
		public StateBag() : this(false)
		{
		}

		/*
		 * Constructs an StateBag
		 */

		/// <devdoc>
		/// <para>Initializes a new instance of the <see cref='System.Web.UI.StateBag'/> class that allows stored state 
		///    values to be case-insensitive.</para>
		/// </devdoc>
		public StateBag(bool ignoreCase)
		{
			marked = false;
			this.ignoreCase = ignoreCase;
			bag = CreateBag();
		}


		/*
		 * Return count of number of StateItems in the bag.
		 */

		/// <devdoc>
		/// <para>Indicates the number of items in the <see cref='System.Web.UI.StateBag'/> object. This property is 
		///    read-only.</para>
		/// </devdoc>
		public int Count
		{
			get
			{
				return bag.Count;
			}
		}

		/*
		 * Returns a collection of keys.
		 */

		/// <devdoc>
		/// <para>Indicates a collection of keys representing the items in the <see cref='System.Web.UI.StateBag'/> object. 
		///    This property is read-only.</para>
		/// </devdoc>
		public ICollection Keys
		{
			get
			{
				return bag.Keys;
			}
		}

		/*
		 * Returns a collection of values.
		 */

		/// <devdoc>
		/// <para>Indicates a collection of view state values in the <see cref='System.Web.UI.StateBag'/> object. 
		///    This property is read-only.</para>
		/// </devdoc>
		public ICollection Values
		{
			get
			{
				return bag.Values;
			}
		}

		/*
		 * Get or set value of a StateItem.
		 * A set will automatically add a new StateItem for a
		 * key which is not already in the bag.  A set to null
		 * will remove the item if set before mark, otherwise
		 * a null set will be saved to allow tracking of state
		 * removed after mark.
		 */

		/// <devdoc>
		///    <para> Indicates the value of an item stored in the 
		///    <see langword='StateBag'/> 
		///    object. Setting this property with a key not already stored in the StateBag will
		///    add an item to the bag. If you set this property to <see langword='null'/> before
		///    the TrackState method is called on an item will remove it from the bag. Otherwise,
		///    when you set this property to <see langword='null'/>
		///    the key will be saved to allow tracking of the item's state.</para>
		/// </devdoc>
		public object this[string key]
		{
			get
			{
				if (String.IsNullOrEmpty(key))
					throw ExceptionUtil.ParameterNullOrEmpty("key");

				var item = bag[key] as StateItem;
				if (item != null)
					return item.Value;
				return null;
			}
			set
			{
				Add(key, value);
			}
		}

		/*
		 * Private implementation of IDictionary item accessor
		 */

		/// <internalonly/>
		object IDictionary.this[object key]
		{
			get { return this[(string)key]; }
			set { this[(string)key] = value; }
		}

		private IDictionary CreateBag()
		{
			return new HybridDictionary(ignoreCase);
		}

		/*
		 * Add a new StateItem or update an existing StateItem in the bag.
		 */

		/// <devdoc>
		///    <para>[To be supplied.]</para>
		/// </devdoc>
		public StateItem Add(string key, object value)
		{

			if (String.IsNullOrEmpty(key))
				throw ExceptionUtil.ParameterNullOrEmpty("key");

			var item = bag[key] as StateItem;

			if (item == null)
			{
				if (value != null || marked)
				{
					item = new StateItem(value);
					bag.Add(key, item);
				}
			}
			else
			{
				if (value == null && !marked)
				{
					bag.Remove(key);
				}
				else
				{
					item.Value = value;
				}
			}
			if (item != null && marked)
			{
				item.IsDirty = true;
			}
			return item;
		}

		/*
		 * Private implementation of IDictionary Add
		 */

		/// <internalonly/>
		void IDictionary.Add(object key, object value)
		{
			Add((string)key, value);
		}

		/*
		 * Clear all StateItems from the bag.
		 */

		/// <devdoc>
		/// <para>Removes all controls from the <see cref='System.Web.UI.StateBag'/> object.</para>
		/// </devdoc>
		public void Clear()
		{
			bag.Clear();
		}

		/*
		 * Get an enumerator for the StateItems.
		 */

		/// <devdoc>
		///    <para>Returns an enumerator that iterates over the key/value pairs stored in 
		///       the <see langword='StateBag'/>.</para>
		/// </devdoc>
		public IDictionaryEnumerator GetEnumerator()
		{
			return bag.GetEnumerator();
		}

		/*
		 * Return the dirty flag of the state item.
		 * Returns false if there is not an item for given key.
		 */

		/// <devdoc>
		/// <para>Checks an item stored in the <see langword='StateBag'/> to see if it has been 
		///    modified.</para>
		/// </devdoc>
		public bool IsItemDirty(string key)
		{
			var item = bag[key] as StateItem;
			if (item != null)
				return item.IsDirty;

			return false;
		}

		/*
		 * Return true if 'marked' and state changes are being tracked.
		 */

		/// <devdoc>
		///    <para>Determines if state changes in the StateBag object's store are being tracked.</para>
		/// </devdoc>
		internal bool IsTrackingViewState
		{
			get
			{
				return marked;
			}
		}

		/*
		 * Restore state that was previously saved via SaveViewState.
		 */

		/// <devdoc>
		///    <para>Loads the specified previously saved state information</para>
		/// </devdoc>
		internal void LoadViewState(object state)
		{
			if (state != null)
			{
				var data = (ArrayList)state;

				for (var i = 0; i < data.Count; i += 2)
				{
#if OBJECTSTATEFORMATTER
                    string key = ((IndexedString)data[i]).Value;
#else
					var key = (string)data[i];
#endif
					var value = data[i + 1];

					Add(key, value);
				}
			}
		}

		/*
		 * Start tracking state changes after "mark".
		 */

		/// <devdoc>
		///    <para>Initiates the tracking of state changes for items stored in the 
		///    <see langword='StateBag'/> object.</para>
		/// </devdoc>
		internal void TrackViewState()
		{
			marked = true;
		}

		/*
		 * Remove a StateItem from the bag altogether regardless of marked.
		 * Used internally by controls.
		 */

		/// <devdoc>
		/// <para>Removes the specified item from the <see cref='System.Web.UI.StateBag'/> object.</para>
		/// </devdoc>
		public void Remove(string key)
		{
			bag.Remove(key);
		}

		/*
		 * Private implementation of IDictionary Remove
		 */

		/// <internalonly/>
		void IDictionary.Remove(object key)
		{
			Remove((string)key);
		}


		/*
		 * Return object containing state that has been modified since "mark".
		 * Returns null if there is no modified state.
		 */

		/// <devdoc>
		///    <para>Returns an object that contains all state changes for items stored in the 
		///    <see langword='StateBag'/> object.</para>
		/// </devdoc>
		internal object SaveViewState()
		{
			ArrayList data = null;

			// 

			if (bag.Count != 0)
			{
				var e = bag.GetEnumerator();
				while (e.MoveNext())
				{
					var item = (StateItem)(e.Value);
					if (item.IsDirty)
					{
						if (data == null)
						{
							data = new ArrayList();
						}
#if OBJECTSTATEFORMATTER
                        data.Add(new IndexedString((string)e.Key));
#else
						data.Add(e.Key);
#endif
						data.Add(item.Value);
					}
				}
			}

			return data;
		}


		/// <devdoc>
		/// Sets the dirty state of all item values currently in the StateBag.
		/// </devdoc>
		public void SetDirty(bool dirty)
		{
			if (bag.Count != 0)
			{
				foreach (StateItem item in bag.Values)
				{
					item.IsDirty = dirty;
				}
			}
		}

		/*
		 * Internal method for setting dirty flag on a state item.
		 * Used internallly to prevent state management of certain properties.
		 */

		/// <internalonly/>
		/// <devdoc>
		/// </devdoc>
		public void SetItemDirty(string key, bool dirty)
		{
			var item = bag[key] as StateItem;
			if (item != null)
			{
				item.IsDirty = dirty;
			}
		}

		/// <internalonly/>
		bool IDictionary.IsFixedSize
		{
			get { return false; }
		}


		/// <internalonly/>
		bool IDictionary.IsReadOnly
		{
			get { return false; }
		}


		/// <internalonly/>
		bool ICollection.IsSynchronized
		{
			get { return false; }
		}


		/// <internalonly/>
		object ICollection.SyncRoot
		{
			get { return this; }
		}


		/// <internalonly/>
		bool IDictionary.Contains(object key)
		{
			return bag.Contains((string)key);
		}


		/// <internalonly/>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IDictionary)this).GetEnumerator();
		}


		/// <internalonly/>
		void ICollection.CopyTo(Array array, int index)
		{
			Values.CopyTo(array, index);
		}


		/*
		 * Return true if tracking state changes.
		 * Method of private interface, IStateManager.
		 */

		/// <internalonly/>
		bool IStateManager.IsTrackingViewState
		{
			get
			{
				return IsTrackingViewState;
			}
		}

		/*
		 * Load previously saved state.
		 * Method of private interface, IStateManager.
		 */

		/// <internalonly/>
		void IStateManager.LoadViewState(object state)
		{
			LoadViewState(state);
		}

		/*
		 * Start tracking state changes.
		 * Method of private interface, IStateManager.
		 */

		/// <internalonly/>
		void IStateManager.TrackViewState()
		{
			TrackViewState();
		}

		/*
		 * Return object containing state changes.
		 * Method of private interface, IStateManager.
		 */

		/// <internalonly/>
		object IStateManager.SaveViewState()
		{
			return SaveViewState();
		}
	}

	[TypeConverterAttribute(typeof(ExpandableObjectConverter)), DefaultProperty("HeaderText")]
	public abstract class DataControlField : IStateManager, IDataSourceViewSchemaAccessor
	{
		private TableItemStyle _itemStyle;
		private TableItemStyle _headerStyle;
		private TableItemStyle _footerStyle;
		private Style _controlStyle;
		private StateBag _statebag;
		private bool _trackViewState;
		private bool _sortingEnabled;
		private Control _control;
		private object _dataSourceViewSchema;

		internal event EventHandler FieldChanged;



		/// <devdoc>
		/// <para>Initializes a new instance of the System.Web.UI.WebControls.Field class.</para>
		/// </devdoc>
		protected DataControlField()
		{
			_statebag = new StateBag();
			_dataSourceViewSchema = null;
		}


		/// <devdoc>
		/// <para>Gets or sets the text rendered as the AbbreviatedText in some controls.</para>
		/// </devdoc>
		[
		Localizable(true),
		WebCategory("Accessibility"),
		DefaultValue(""),
		WebSysDescription(SR.DataControlField_AccessibleHeaderText)
		]
		public virtual string AccessibleHeaderText
		{
			get
			{
				object o = ViewState["AccessibleHeaderText"];
				if (o != null)
					return (string)o;
				return String.Empty;
			}
			set
			{
				if (!String.Equals(value, ViewState["AccessibleHeaderText"]))
				{
					ViewState["AccessibleHeaderText"] = value;
					OnFieldChanged();
				}
			}
		}


		/// <devdoc>
		/// <para>Gets the style properties for the controls inside this field.</para>
		/// </devdoc>
		[
		WebCategory("Styles"),
		DefaultValue(null),
		WebSysDescription(SR.DataControlField_ControlStyle),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		PersistenceMode(PersistenceMode.InnerProperty)
		]
		public Style ControlStyle
		{
			get
			{
				if (_controlStyle == null)
				{
					_controlStyle = new Style();
					if (IsTrackingViewState)
						((IStateManager)_controlStyle).TrackViewState();
				}
				return _controlStyle;
			}
		}

		/// <summary>
		/// This property is accessed by <see cref='System.Web.UI.WebControls.DataControlFieldCell'/>.
		/// Any child classes which define <see cref='System.Web.UI.Control.ValidateRequestMode'/> should be wrapping this so that
		/// <see cref='System.Web.UI.WebControls.DataControlFieldCell'/> gets the correct value.
		/// </summary>
		protected internal virtual ValidateRequestMode ValidateRequestMode
		{
			get
			{
				object o = ViewState["ValidateRequestMode"];
				if (o != null)
					return (ValidateRequestMode)o;
				return ValidateRequestMode.Inherit;
			}
			set
			{
				if (value < ValidateRequestMode.Inherit || value > ValidateRequestMode.Enabled)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (value != ValidateRequestMode)
				{
					ViewState["ValidateRequestMode"] = value;
					OnFieldChanged();
				}
			}
		}

		internal Style ControlStyleInternal
		{
			get
			{
				return _controlStyle;
			}
		}

		protected Control Control
		{
			get
			{
				return _control;
			}
		}


		/// <devdoc>
		/// <para>[To be supplied.]</para>
		/// </devdoc>
		protected bool DesignMode
		{
			get
			{
				if (_control != null)
				{
					return _control.DesignMode;
				}
				return false;
			}
		}


		/// <devdoc>
		/// <para>Gets the style properties for the footer item.</para>
		/// </devdoc>
		[
		WebCategory("Styles"),
		DefaultValue(null),
		WebSysDescription(SR.DataControlField_FooterStyle),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		PersistenceMode(PersistenceMode.InnerProperty)
		]
		public TableItemStyle FooterStyle
		{
			get
			{
				if (_footerStyle == null)
				{
					_footerStyle = new TableItemStyle();
					if (IsTrackingViewState)
						((IStateManager)_footerStyle).TrackViewState();
				}
				return _footerStyle;
			}
		}

		/// <devdoc>
		/// </devdoc>
		internal TableItemStyle FooterStyleInternal
		{
			get
			{
				return _footerStyle;
			}
		}


		/// <devdoc>
		/// <para> Gets or sets the text displayed in the footer of the
		/// System.Web.UI.WebControls.Field.</para>
		/// </devdoc>
		[
		Localizable(true),
		WebCategory("Appearance"),
		DefaultValue(""),
		WebSysDescription(SR.DataControlField_FooterText)
		]
		public virtual string FooterText
		{
			get
			{
				object o = ViewState["FooterText"];
				if (o != null)
					return (string)o;
				return String.Empty;
			}
			set
			{
				if (!String.Equals(value, ViewState["FooterText"]))
				{
					ViewState["FooterText"] = value;
					OnFieldChanged();
				}
			}
		}


		/// <devdoc>
		/// <para>Gets or sets the URL reference to an image to display
		/// instead of text on the header of this System.Web.UI.WebControls.Field
		/// .</para>
		/// </devdoc>
		[
		WebCategory("Appearance"),
		DefaultValue(""),
		Editor("System.Web.UI.Design.ImageUrlEditor, " + AssemblyRef.SystemDesign, typeof(UITypeEditor)),
		UrlProperty(),
		WebSysDescription(SR.DataControlField_HeaderImageUrl)
		]
		public virtual string HeaderImageUrl
		{
			get
			{
				object o = ViewState["HeaderImageUrl"];
				if (o != null)
					return (string)o;
				return String.Empty;
			}
			set
			{
				if (!String.Equals(value, ViewState["HeaderImageUrl"]))
				{
					ViewState["HeaderImageUrl"] = value;
					OnFieldChanged();
				}
			}
		}


		/// <devdoc>
		/// <para>Gets the style properties for the header of the System.Web.UI.WebControls.Field. This property is read-only.</para>
		/// </devdoc>
		[
		WebCategory("Styles"),
		DefaultValue(null),
		WebSysDescription(SR.DataControlField_HeaderStyle),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		PersistenceMode(PersistenceMode.InnerProperty)
		]
		public TableItemStyle HeaderStyle
		{
			get
			{
				if (_headerStyle == null)
				{
					_headerStyle = new TableItemStyle();
					if (IsTrackingViewState)
						((IStateManager)_headerStyle).TrackViewState();
				}
				return _headerStyle;
			}
		}

		/// <devdoc>
		/// </devdoc>
		internal TableItemStyle HeaderStyleInternal
		{
			get
			{
				return _headerStyle;
			}
		}


		/// <devdoc>
		/// <para>Gets or sets the text displayed in the header of the
		/// System.Web.UI.WebControls.Field.</para>
		/// </devdoc>
		[
		Localizable(true),
		WebCategory("Appearance"),
		DefaultValue(""),
		WebSysDescription(SR.DataControlField_HeaderText)
		]
		public virtual string HeaderText
		{
			get
			{
				object o = ViewState["HeaderText"];
				if (o != null)
					return (string)o;
				return String.Empty;
			}
			set
			{
				if (!String.Equals(value, ViewState["HeaderText"]))
				{
					ViewState["HeaderText"] = value;
					OnFieldChanged();
				}
			}
		}

		/// <devdoc>
		///    <para>Gets or sets whether the field is visible in Insert mode.  Turn off for auto-gen'd db fields</para>
		/// </devdoc>
		[
				WebCategory("Behavior"),
				DefaultValue(true),
				WebSysDescription(SR.DataControlField_InsertVisible)
		]
		public virtual bool InsertVisible
		{
			get
			{
				object o = ViewState["InsertVisible"];
				if (o != null)
					return (bool)o;
				return true;
			}
			set
			{
				object oldValue = ViewState["InsertVisible"];
				if (oldValue == null || value != (bool)oldValue)
				{
					ViewState["InsertVisible"] = value;
					OnFieldChanged();
				}
			}
		}


		/// <devdoc>
		/// <para>Gets the style properties of an item within the System.Web.UI.WebControls.Field. This property is read-only.</para>
		/// </devdoc>
		[
		WebCategory("Styles"),
		DefaultValue(null),
		WebSysDescription(SR.DataControlField_ItemStyle),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		PersistenceMode(PersistenceMode.InnerProperty)
		]
		public TableItemStyle ItemStyle
		{
			get
			{
				if (_itemStyle == null)
				{
					_itemStyle = new TableItemStyle();
					if (IsTrackingViewState)
						((IStateManager)_itemStyle).TrackViewState();
				}
				return _itemStyle;
			}
		}

		/// <devdoc>
		/// </devdoc>
		internal TableItemStyle ItemStyleInternal
		{
			get
			{
				return _itemStyle;
			}
		}


		[
		WebCategory("Behavior"),
		DefaultValue(true),
		WebSysDescription(SR.DataControlField_ShowHeader)
		]
		public virtual bool ShowHeader
		{
			get
			{
				object o = ViewState["ShowHeader"];
				if (o != null)
				{
					return (bool)o;
				}
				return true;
			}
			set
			{
				object oldValue = ViewState["ShowHeader"];
				if (oldValue == null || (bool)oldValue != value)
				{
					ViewState["ShowHeader"] = value;
					OnFieldChanged();
				}
			}
		}


		/// <devdoc>
		/// <para>Gets or sets the expression used when this field is used to sort the data source> by.</para>
		/// </devdoc>
		[
		WebCategory("Behavior"),
		DefaultValue(""),
		TypeConverter("System.Web.UI.Design.DataSourceViewSchemaConverter, " + AssemblyRef.SystemDesign),
		WebSysDescription(SR.DataControlField_SortExpression)
		]
		public virtual string SortExpression
		{
			get
			{
				object o = ViewState["SortExpression"];
				if (o != null)
					return (string)o;
				return String.Empty;
			}
			set
			{
				if (!String.Equals(value, ViewState["SortExpression"]))
				{
					ViewState["SortExpression"] = value;
					OnFieldChanged();
				}
			}
		}


		/// <devdoc>
		/// <para>Gets the statebag for the System.Web.UI.WebControls.Field. This property is read-only.</para>
		/// </devdoc>
		protected StateBag ViewState
		{
			get
			{
				return _statebag;
			}
		}


		/// <devdoc>
		/// <para>Gets or sets a value to indicate whether the System.Web.UI.WebControls.Field is visible.</para>
		/// </devdoc>
		[
		WebCategory("Behavior"),
		DefaultValue(true),
		WebSysDescription(SR.DataControlField_Visible)
		]
		public bool Visible
		{
			get
			{
				object o = ViewState["Visible"];
				if (o != null)
					return (bool)o;
				return true;
			}
			set
			{
				object oldValue = ViewState["Visible"];
				if (oldValue == null || value != (bool)oldValue)
				{
					ViewState["Visible"] = value;
					OnFieldChanged();
				}
			}
		}

		protected internal DataControlField CloneField()
		{
			DataControlField newField = CreateField();
			CopyProperties(newField);
			return newField;
		}

		protected virtual void CopyProperties(DataControlField newField)
		{
			newField.AccessibleHeaderText = AccessibleHeaderText;
			newField.ControlStyle.CopyFrom(ControlStyle);
			newField.FooterStyle.CopyFrom(FooterStyle);
			newField.HeaderStyle.CopyFrom(HeaderStyle);
			newField.ItemStyle.CopyFrom(ItemStyle);
			newField.FooterText = FooterText;
			newField.HeaderImageUrl = HeaderImageUrl;
			newField.HeaderText = HeaderText;
			newField.InsertVisible = InsertVisible;
			newField.ShowHeader = ShowHeader;
			newField.SortExpression = SortExpression;
			newField.Visible = Visible;
			newField.ValidateRequestMode = ValidateRequestMode;
		}

		protected abstract DataControlField CreateField();


		/// <devdoc>
		/// Extracts the value of the databound cell and inserts the value into the given dictionary
		/// </devdoc>
		public virtual void ExtractValuesFromCell(IOrderedDictionary dictionary, DataControlFieldCell cell, DataControlRowState rowState, bool includeReadOnly)
		{
			return;
		}


		/// <devdoc>
		/// </devdoc>
		public virtual bool Initialize(bool sortingEnabled, Control control)
		{
			_sortingEnabled = sortingEnabled;
			_control = control;
			return false;
		}


		/// <devdoc>
		/// <para>Initializes a cell in the System.Web.UI.WebControls.Field.</para>
		/// </devdoc>
		public virtual void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex)
		{
			switch (cellType)
			{
				case DataControlCellType.Header:
					{
						WebControl headerControl = null;
						string sortExpression = SortExpression;
						bool sortableHeader = (_sortingEnabled && sortExpression.Length > 0);

						string headerImageUrl = HeaderImageUrl;
						string headerText = HeaderText;
						if (headerImageUrl.Length != 0)
						{
							if (sortableHeader)
							{
								ImageButton sortButton;
								IPostBackContainer container = _control as IPostBackContainer;
								if (container != null)
								{
									sortButton = new DataControlImageButton(container);
									((DataControlImageButton)sortButton).EnableCallback(null);  // no command argument for the callback uses Sort
								}
								else
								{
									sortButton = new ImageButton();
								}

								sortButton.ImageUrl = HeaderImageUrl;
								sortButton.CommandName = DataControlCommands.SortCommandName;
								sortButton.CommandArgument = sortExpression;
								if (!(sortButton is DataControlImageButton))
								{
									sortButton.CausesValidation = false;
								}
								sortButton.AlternateText = headerText;
								headerControl = sortButton;
							}
							else
							{
								Image headerImage = new Image();

								headerImage.ImageUrl = headerImageUrl;
								headerControl = headerImage;
								headerImage.AlternateText = headerText;
							}
						}
						else
						{
							if (sortableHeader)
							{
								LinkButton sortButton;
								IPostBackContainer container = _control as IPostBackContainer;
								if (container != null)
								{
									sortButton = new DataControlLinkButton(container);
									((DataControlLinkButton)sortButton).EnableCallback(null);   // no command argument for the callback uses Sort
								}
								else
								{
									sortButton = new LinkButton();
								}

								sortButton.Text = headerText;
								sortButton.CommandName = DataControlCommands.SortCommandName;
								sortButton.CommandArgument = sortExpression;
								if (!(sortButton is DataControlLinkButton))
								{
									sortButton.CausesValidation = false;
								}
								headerControl = sortButton;
							}
							else
							{
								if (headerText.Length == 0)
								{
									// the browser does not render table borders for cells with nothing
									// in their content, so we add a non-breaking space.
									headerText = "&nbsp;";
								}
								cell.Text = headerText;
							}
						}

						if (headerControl != null)
						{
							cell.Controls.Add(headerControl);
						}
					}
					break;

				case DataControlCellType.Footer:
					{
						string footerText = FooterText;
						if (footerText.Length == 0)
						{
							// the browser does not render table borders for cells with nothing
							// in their content, so we add a non-breaking space.
							footerText = "&nbsp;";
						}

						cell.Text = footerText;
					}
					break;
			}
		}


		/// <devdoc>
		/// <para>Determines if the System.Web.UI.WebControls.Field is marked to save its state.</para>
		/// </devdoc>
		protected bool IsTrackingViewState
		{
			get
			{
				return _trackViewState;
			}
		}


		/// <devdoc>
		/// <para>Loads the state of the System.Web.UI.WebControls.Field.</para>
		/// </devdoc>
		protected virtual void LoadViewState(object savedState)
		{
			if (savedState != null)
			{
				object[] myState = (object[])savedState;

				if (myState[0] != null)
					((IStateManager)ViewState).LoadViewState(myState[0]);
				if (myState[1] != null)
					((IStateManager)ItemStyle).LoadViewState(myState[1]);
				if (myState[2] != null)
					((IStateManager)HeaderStyle).LoadViewState(myState[2]);
				if (myState[3] != null)
					((IStateManager)FooterStyle).LoadViewState(myState[3]);
			}
		}


		/// <devdoc>
		/// <para>Raises the FieldChanged event for a System.Web.UI.WebControls.Field.</para>
		/// </devdoc>
		protected virtual void OnFieldChanged()
		{
			if (FieldChanged != null)
			{
				FieldChanged(this, EventArgs.Empty);
			}
		}


		/// <devdoc>
		/// <para>Saves the current state of the System.Web.UI.WebControls.Field.</para>
		/// </devdoc>
		protected virtual object SaveViewState()
		{
			object propState = ((IStateManager)ViewState).SaveViewState();
			object itemStyleState = (_itemStyle != null) ? ((IStateManager)_itemStyle).SaveViewState() : null;
			object headerStyleState = (_headerStyle != null) ? ((IStateManager)_headerStyle).SaveViewState() : null;
			object footerStyleState = (_footerStyle != null) ? ((IStateManager)_footerStyle).SaveViewState() : null;
			object controlStyleState = (_controlStyle != null) ? ((IStateManager)_controlStyle).SaveViewState() : null;

			if ((propState != null) ||
					(itemStyleState != null) ||
					(headerStyleState != null) ||
					(footerStyleState != null) ||
					(controlStyleState != null))
			{
				return new object[5] {
										propState,
										itemStyleState,
										headerStyleState,
										footerStyleState,
										controlStyleState
								};
			}

			return null;
		}

		internal void SetDirty()
		{
			_statebag.SetDirty(true);
			if (_itemStyle != null)
			{
				_itemStyle.SetDirty();
			}
			if (_headerStyle != null)
			{
				_headerStyle.SetDirty();
			}
			if (_footerStyle != null)
			{
				_footerStyle.SetDirty();
			}
			if (_controlStyle != null)
			{
				_controlStyle.SetDirty();
			}
		}


		/// <internalonly/>
		/// <devdoc>
		/// Return a textual representation of the column for UI-display purposes.
		/// </devdoc>
		public override string ToString()
		{
			string headerText = HeaderText.Trim();
			return headerText.Length > 0 ? headerText : GetType().Name;
		}


		/// <devdoc>
		/// <para>Marks the starting point to begin tracking and saving changes to the
		/// control as part of the control viewstate.</para>
		/// </devdoc>
		protected virtual void TrackViewState()
		{
			_trackViewState = true;
			((IStateManager)ViewState).TrackViewState();
			if (_itemStyle != null)
				((IStateManager)_itemStyle).TrackViewState();
			if (_headerStyle != null)
				((IStateManager)_headerStyle).TrackViewState();
			if (_footerStyle != null)
				((IStateManager)_footerStyle).TrackViewState();
			if (_controlStyle != null)
				((IStateManager)_controlStyle).TrackViewState();
		}

		/// <devdoc>
		/// <para>Override with an empty body if the field's controls all support callback.
		///  Otherwise, override and throw a useful error message about why the field can't support callbacks.</para>
		/// </devdoc>
		public virtual void ValidateSupportsCallback()
		{
			throw new NotSupportedException(SR.GetString(SR.DataControlField_CallbacksNotSupported, Control.ID));
		}


		/// <internalonly/>
		/// <devdoc>
		/// Return true if tracking state changes.
		/// </devdoc>
		bool IStateManager.IsTrackingViewState
		{
			get
			{
				return IsTrackingViewState;
			}
		}


		/// <internalonly/>
		/// <devdoc>
		/// Load previously saved state.
		/// </devdoc>
		void IStateManager.LoadViewState(object state)
		{
			LoadViewState(state);
		}


		/// <internalonly/>
		/// <devdoc>
		/// Start tracking state changes.
		/// </devdoc>
		void IStateManager.TrackViewState()
		{
			TrackViewState();
		}


		/// <internalonly/>
		/// <devdoc>
		/// Return object containing state changes.
		/// </devdoc>
		object IStateManager.SaveViewState()
		{
			return SaveViewState();
		}

		#region IDataSourceViewSchemaAccessor implementation

		/// <internalonly/>
		object IDataSourceViewSchemaAccessor.DataSourceViewSchema
		{
			get
			{
				return _dataSourceViewSchema;
			}
			set
			{
				_dataSourceViewSchema = value;
			}
		}
		#endregion
	}

	/// <summary>
	/// The GridView Column Generator
	/// </summary>
	public interface IDataSourceViewSchemaAccessor
	{

		/// <devdoc>
		/// Returns the schema associated with the object implementing this interface.
		/// </devdoc>
		object DataSourceViewSchema { get; set; }
	}

	public interface IStateManager
	{
		/*
     * Return true if tracking state changes.
     */

		/// <devdoc>
		///    <para>Determines if state changes are being tracked.</para>
		///    </devdoc>
		bool IsTrackingViewState
		{
			get;
		}

		/*
     * Load previously saved state.
     */

		/// <devdoc>
		///    <para>Loads the specified control's previously saved state.</para>
		///    </devdoc>
		void LoadViewState(object state);

		/*
     * Return object containing state changes.
     */

		/// <devdoc>
		///    <para>Returns the object that contains the state changes.</para>
		///    </devdoc>
		object SaveViewState();

		/*
     * Start tracking state changes.
     */

		/// <devdoc>
		///    <para>Instructs the control to start tracking changes in state.</para>
		///    </devdoc>
		void TrackViewState();


	}

	public interface IAutoFieldGenerator
	{
		ICollection GenerateFields(Control control);
	}

	public abstract class AutoFieldsGenerator : IAutoFieldGenerator, IStateManager
	{
		private List<AutoGeneratedFieldProperties> _autoGeneratedFieldProperties;
		private bool _tracking;

		internal bool InDataBinding
		{
			get;
			set;
		}

		internal object DataItem
		{
			get;
			set;
		}

		private ICollection CreateAutoGeneratedFieldsFromFieldProperties()
		{
			List<AutoGeneratedField> autoFields = new List<AutoGeneratedField>();

			foreach (AutoGeneratedFieldProperties fieldProperties in AutoGeneratedFieldProperties)
			{
				autoFields.Add(CreateAutoGeneratedFieldFromFieldProperties(fieldProperties));
			}

			return autoFields;
		}

		//internal for unit testing.
		protected internal List<AutoGeneratedFieldProperties> AutoGeneratedFieldProperties
		{
			get
			{
				if (_autoGeneratedFieldProperties == null)
				{
					_autoGeneratedFieldProperties = new List<AutoGeneratedFieldProperties>();
				}
				return _autoGeneratedFieldProperties;
			}
		}

		/// <devdoc>
		/// Create a single autogenerated field.  This function can be overridden to create a different AutoGeneratedField.
		/// </devdoc>
		protected virtual AutoGeneratedField CreateAutoGeneratedFieldFromFieldProperties(AutoGeneratedFieldProperties fieldProperties)
		{
			AutoGeneratedField field = new AutoGeneratedField(fieldProperties.DataField);
			string name = fieldProperties.Name;
			((IStateManager)field).TrackViewState();

			field.HeaderText = name;
			field.SortExpression = name;
			field.ReadOnly = fieldProperties.IsReadOnly;
			field.DataType = fieldProperties.Type;

			return field;
		}

		protected bool IsTrackingViewState
		{
			get
			{
				return _tracking;
			}
		}

		protected virtual void TrackViewState()
		{
			_tracking = true;
			foreach (object fieldProps in AutoGeneratedFieldProperties)
			{
				((IStateManager)fieldProps).TrackViewState();
			}
		}

		protected virtual void LoadViewState(object savedState)
		{
			if (savedState != null)
			{
				object[] autoGenFieldStateArray = (object[])savedState;
				int fieldCount = autoGenFieldStateArray.Length;
				AutoGeneratedFieldProperties.Clear();

				for (int i = 0; i < fieldCount; i++)
				{
					AutoGeneratedFieldProperties fieldProps = new AutoGeneratedFieldProperties();
					((IStateManager)fieldProps).TrackViewState();
					((IStateManager)fieldProps).LoadViewState(autoGenFieldStateArray[i]);
					AutoGeneratedFieldProperties.Add(fieldProps);
				}
			}
		}

		protected virtual object SaveViewState()
		{
			// pack up the auto gen'd field properties
			int autoGenFieldPropsCount = AutoGeneratedFieldProperties.Count;
			object[] autoGenFieldState = new object[autoGenFieldPropsCount];

			for (int i = 0; i < autoGenFieldPropsCount; i++)
			{
				autoGenFieldState[i] = ((IStateManager)AutoGeneratedFieldProperties[i]).SaveViewState();
			}
			return autoGenFieldState;
		}

		/// <summary>
		/// Method that creates the AutoGeneratedFields. This would also typically populate the AutoGeneratedFieldProperties collection
		/// which are tracked by view state.
		/// </summary>
		/// <returns></returns>
		public abstract List<AutoGeneratedField> CreateAutoGeneratedFields(object dataItem, Control control);

		/// <summary>
		/// A flag to indicate whether Enum fields should be generated by this FieldGenerator.
		/// If this flag is not set, the FieldGenerator uses <see cref='System.Web.UI.WebControls.Control.RenderingCompatibility'/> of
		/// <see cref='System.Web.UI.WebControls.Control'/> to determine the default behavior which is 
		/// true when RenderingCompatibility is greater than or equal to 4.5 and false in older versions.
		/// </summary>
		public bool? AutoGenerateEnumFields
		{
			get;
			set;
		}

		#region Implementation of IAutoFieldGenerator

		public virtual ICollection GenerateFields(Control control)
		{
			if (InDataBinding)
			{
				//Reset this collection as this would be populated by CreateAutoGenerateFields.
				_autoGeneratedFieldProperties = null;
				return CreateAutoGeneratedFields(DataItem, control);
			}
			else
			{
				return CreateAutoGeneratedFieldsFromFieldProperties();
			}
		}

		#endregion

		#region Implementation of IStateManager

		bool IStateManager.IsTrackingViewState
		{
			get
			{
				return IsTrackingViewState;
			}
		}

		void IStateManager.LoadViewState(object savedState)
		{
			LoadViewState(savedState);
		}

		object IStateManager.SaveViewState()
		{
			return SaveViewState();
		}

		void IStateManager.TrackViewState()
		{
			TrackViewState();
		}

		#endregion
	}

	public class GridViewColumnsGenerator : AutoFieldsGenerator
	{
		public override List<AutoGeneratedField> CreateAutoGeneratedFields(object dataObject, Control control)
		{
			if (!(control is GridView))
			{
				throw new ArgumentException(SR.GetString(SR.InvalidDefaultAutoFieldGenerator, GetType().FullName, typeof(GridView).FullName));
			}

			Debug.Assert(dataObject == null || dataObject is PagedDataSource);

			PagedDataSource dataSource = dataObject as PagedDataSource;
			GridView gridView = control as GridView;

			if (dataSource == null)
			{
				// note that we're not throwing an exception in this case, and the calling
				// code should be able to handle a null arraylist being returned
				return null;
			}

			List<AutoGeneratedField> generatedFields = new List<AutoGeneratedField>();
			PropertyDescriptorCollection propDescs = null;
			bool throwException = true;

			// try ITypedList first
			// A PagedDataSource implements this, but returns null, if the underlying data source
			// does not implement it.
			propDescs = ((ITypedList)dataSource).GetItemProperties(new PropertyDescriptor[0]);

			if (propDescs == null)
			{
				Type sampleItemType = null;
				object sampleItem = null;

				IEnumerable realDataSource = dataSource.DataSource;
				Debug.Assert(realDataSource != null, "Must have a real data source when calling CreateAutoGeneratedColumns");

				Type dataSourceType = realDataSource.GetType();

				// try for a typed Row property, which should be present on strongly typed collections
				PropertyInfo itemProp = dataSourceType.GetProperty("Item", BindingFlags.Public | BindingFlags.Instance, null, null, new Type[] { typeof(int) }, null);
				if (itemProp != null)
				{
					sampleItemType = itemProp.PropertyType;
				}

				if ((sampleItemType == null) || (sampleItemType == typeof(object)))
				{
					// last resort... try to get ahold of the first item by beginning the
					// enumeration

					IEnumerator e = dataSource.GetEnumerator();

					if (e.MoveNext())
					{
						sampleItem = e.Current;
					}
					else
					{
						// we don't want to throw an exception if we're bound to an IEnumerable
						// data source with no records... we'll simply bail and not show any data
						throwException = false;
					}
					if (sampleItem != null)
					{
						sampleItemType = sampleItem.GetType();
					}

					// We must store the enumerator regardless of whether we got back an item from it
					// because we cannot start the enumeration again, in the case of a DataReader.
					// Code in CreateChildControls must deal appropriately for the case where
					// there is a stored enumerator, but a null object as the first item.
					gridView.StoreEnumerator(e, sampleItem);
				}

				if ((sampleItem != null) && (sampleItem is ICustomTypeDescriptor))
				{
					// Get the custom properties of the object
					propDescs = TypeDescriptor.GetProperties(sampleItem);
				}
				else if (sampleItemType != null)
				{
					// directly bindable types: strings, ints etc. get treated specially, since we
					// don't care about their properties, but rather we care about them directly
					if (ShouldGenerateField(sampleItemType, gridView))
					{
						AutoGeneratedFieldProperties fieldProps = new AutoGeneratedFieldProperties();
						((IStateManager)fieldProps).TrackViewState();

						fieldProps.Type = sampleItemType;
						fieldProps.Name = "Item";
						fieldProps.DataField = AutoGeneratedField.ThisExpression;

						AutoGeneratedField field = CreateAutoGeneratedFieldFromFieldProperties(fieldProps);
						if (field != null)
						{
							generatedFields.Add(field);

							AutoGeneratedFieldProperties.Add(fieldProps);
						}
					}
					else
					{
						// complex type... we get its properties
						propDescs = TypeDescriptor.GetProperties(sampleItemType);
					}
				}
			}
			else
			{
				if (propDescs.Count == 0)
				{
					// we don't want to throw an exception if we're bound to an ITypedList
					// data source with no records... we'll simply bail and not show any data
					throwException = false;
				}
			}

			if ((propDescs != null) && (propDescs.Count != 0))
			{
				string[] dataKeyNames = gridView.DataKeyNames;
				int keyNamesLength = dataKeyNames.Length;
				string[] dataKeyNamesCaseInsensitive = new string[keyNamesLength];
				for (int i = 0; i < keyNamesLength; i++)
				{
					dataKeyNamesCaseInsensitive[i] = dataKeyNames[i].ToLowerInvariant();
				}
				foreach (PropertyDescriptor pd in propDescs)
				{
					Type propertyType = pd.PropertyType;
					if (ShouldGenerateField(propertyType, gridView))
					{
						string name = pd.Name;
						bool isKey = ((IList)dataKeyNamesCaseInsensitive).Contains(name.ToLowerInvariant());
						AutoGeneratedFieldProperties fieldProps = new AutoGeneratedFieldProperties();
						((IStateManager)fieldProps).TrackViewState();
						fieldProps.Name = name;
						fieldProps.IsReadOnly = isKey;
						fieldProps.Type = propertyType;
						fieldProps.DataField = name;

						AutoGeneratedField field = CreateAutoGeneratedFieldFromFieldProperties(fieldProps);
						if (field != null)
						{
							generatedFields.Add(field);
							AutoGeneratedFieldProperties.Add(fieldProps);
						}
					}
				}
			}

			if ((generatedFields.Count == 0) && throwException)
			{
				// this handles the case where we got back something that either had no
				// properties, or all properties were not bindable.
				throw new InvalidOperationException(SR.GetString(SR.GridView_NoAutoGenFields, gridView.ID));
			}

			return generatedFields;
		}

		private bool ShouldGenerateField(Type propertyType, GridView gridView)
		{
			if (gridView.RenderingCompatibility < VersionUtil.Framework45 && AutoGenerateEnumFields == null)
			{
				//This is for backward compatibility. Before 4.5, auto generating fields used to call into this method
				//and if someone has overriden this method to force generation of columns, the scenario should still
				//work.
				return gridView.IsBindableType(propertyType);
			}
			else
			{
				//If AutoGenerateEnumFileds is null here, the rendering compatibility must be 4.5
				return DataBoundControlHelper.IsBindableType(propertyType, enableEnums: AutoGenerateEnumFields ?? true);
			}
		}

	}
	public static class GridViewColumnGenerator
	{
		/// <summary>
		/// Generate columns for a given GridView based on the properties of given Type
		/// </summary>
		/// <typeparam name="ItemType"> The type </typeparam>
		/// <param name="gridView"> The GridView </param>
		public static void GenerateColumns<ItemType>(GridView<ItemType> gridView)
		{
			var type = typeof(ItemType);
			var propertiesInfo = type.GetProperties(BindingFlags.Instance | BindingFlags.Public).OrderBy(x => x.MetadataToken);
			foreach (var propertyInfo in propertiesInfo)
			{
				var newColumn = new BoundField
				{
					DataField = propertyInfo.Name,
					ParentColumnsCollection = gridView,
					HeaderText = propertyInfo.Name
				};
				gridView.ColumnList.Add(newColumn);
			}
		}
	}
}
