<script setup lang="ts">
import { RouterLink } from "vue-router";

defineProps<{
  isSpa: boolean;
}>();
</script>

<template>
  <nav>
    <a href="/">Home</a>

    <RouterLink v-if="isSpa" to="/admin">Admin home</RouterLink>
    <a v-else href="/admin">Admin home</a>

    <RouterLink v-if="isSpa" to="/admin/about">Admin about</RouterLink>
    <a v-else href="/admin/about">Admin about</a>

    <RouterLink v-if="isSpa" to="/admin/test1">Admin Test 1</RouterLink>
    <a v-else href="/admin/test1">Admin Test 1</a>

    <a href="/admin/test2">Admin Test 2</a>
  </nav>
</template>

<style scoped>
nav {
  width: 100%;
  font-size: 12px;
  text-align: center;
  margin-top: 2rem;
}

nav a.router-link-exact-active {
  color: var(--color-text);
}

nav a.router-link-exact-active:hover {
  background-color: transparent;
}

nav a {
  display: inline-block;
  padding: 0 1rem;
  border-left: 1px solid var(--color-border);
}

nav a:first-of-type {
  border: 0;
}

@media (min-width: 1024px) {
  nav {
    text-align: left;
    margin-left: -1rem;
    font-size: 1rem;

    padding: 1rem 0;
    margin-top: 1rem;
  }
}
</style>
