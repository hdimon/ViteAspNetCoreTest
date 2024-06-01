<script setup lang="ts">
import { RouterLink } from "vue-router";

defineProps<{
  isSpa: boolean;
}>();
</script>

<template>
  <nav>
    <RouterLink v-if="isSpa" to="/">Home</RouterLink>
    <a v-else href="/">Home</a>

    <RouterLink v-if="isSpa" to="/about">About</RouterLink>
    <a v-else href="/about">About</a>

    <a href="/privacy">Privacy</a>
    <a href="/admin">Admin</a>

    <RouterLink v-if="isSpa" to="/test1">Test 1</RouterLink>
    <a v-else href="/test1">Test 1</a>

    <a href="/test2">Test 2</a>
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
