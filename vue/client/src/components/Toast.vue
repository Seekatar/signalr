<template>
  <div v-if="visible" class="toast" :class="type">
    {{ timestamp }}
    {{ message }}
  </div>
</template>

<script lang="ts">
export default {
  name: 'Toast-popup',
  props: {
    timestamp: String,
    message: String,
    type: {
      type: String,
      default: 'info',
    },
  },
  data() {
    return {
      visible: true,
    };
  },
  mounted() {
    setTimeout(() => {
      this.visible = false;
    }, 5000); // Toast will disappear after 5 seconds
  },
};
</script>

<style scoped>
.toast {
  position: fixed;
  bottom: 20px;
  right: 20px;
  padding: 10px 20px;
  background: #333;
  color: white;
  border-radius: 5px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.2);
  animation: fadeIn 0.3s;
}
.toast.info {
  background: #007bff;
}
.toast.warning {
  background: #f1e430;
}
.toast.error {
  background: #dc3545;
}
.toast.success {
  background: #28a745;
}
@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(10px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}
</style>
