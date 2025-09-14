import { RegisterForm } from "@/components/auth/register-form";
import AuthShell from "@/components/auth/auth-shell.tsx";

export default function RegisterPage() {
  return (
    <AuthShell>
      <RegisterForm />
    </AuthShell>
  );
}
