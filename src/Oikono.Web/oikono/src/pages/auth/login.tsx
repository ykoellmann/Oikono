import { LoginForm } from "@/components/auth/login-form";
import AuthShell from "@/components/auth/auth-shell.tsx";

export default function LoginPage() {
  return (
    <AuthShell>
      <LoginForm />
    </AuthShell>
  );
}
