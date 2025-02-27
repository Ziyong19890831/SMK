<input type="text" class="maskdate" size="10" maxlength="10" placeholder="JJ.MM.AAAA"
       v-bind:value="value"
       v-on:input="$emit('input', $event.target.value)" />

Vue.component('datepicker', {
  props: ['value']
  mounted: function() {
    // activate the plugin when the component is mounted.
    $(this.$el).datepicker({
      dateFormat: 'dd.mm.yy',
      onClose: this.onClose
  },
  methods: {
    // callback for when the selector popup is closed.
    onClose(date) {
      this.$emit('input', date) //maybe you need to validate something here? Dont knwo the plugin well enough.
    }
  },
  watch: {
    // when the value fo the input is changed from the parent,
    // the value prop will update, and we pass that updated value to the plugin.
    value(newVal) { $(this.el).datepicker('setDate', newVal); }
  }
}