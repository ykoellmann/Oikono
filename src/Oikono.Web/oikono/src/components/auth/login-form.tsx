import * as React from "react";
import { z } from "zod";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { cn } from "@/lib/utils";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { useAuth } from "@/auth/AuthContext";
import { useLocation, useNavigate, Link } from "react-router-dom";
import { FormLabel, FormMessage } from "@/components/ui/form";

const LoginSchema = z.object({
  email: z.string().min(1, "Email is required").email("Invalid email").max(254, "Email too long"),
  password: z.string().min(1, "Password is required"),
});

type LoginValues = z.infer<typeof LoginSchema>;

export function LoginForm({ className, ...props }: React.ComponentProps<"form">) {
  const { login } = useAuth();
  const [error, setError] = React.useState<string | null>(null);
  const navigate = useNavigate();
  const location = useLocation() as { state?: { from?: { pathname?: string } } };
  const from = location.state?.from?.pathname || "/";

  const form = useForm<LoginValues>({
    resolver: zodResolver(LoginSchema),
    defaultValues: { email: "", password: "" },
    mode: "onTouched",
  });

  async function onSubmit(values: LoginValues) {
    setError(null);
    try {
      await login(values.email, values.password);
      navigate(from, { replace: true });
    } catch (e) {
      const err = e as { response?: { data?: { title?: string } } };
      setError(err?.response?.data?.title || "Login failed");
    }
  }

  function oauth(provider: "google" | "apple") {
    window.location.href = `/api/authentication/oauth/${provider}/challenge`;
  }

  return (
    <form className={cn("flex flex-col gap-6", className)} onSubmit={form.handleSubmit(onSubmit)} {...props}>
      <div className="flex flex-col gap-2">
        <h1 className="text-2xl font-bold">Login to your account</h1>
        <p className="text-muted-foreground text-sm text-balance">
          Enter your email below to login to your account
        </p>
      </div>
      <div className="grid gap-6">
        <div className="grid gap-3">
          <FormLabel className="text-left" htmlFor="email" invalid={!!form.formState.errors.email}>Email</FormLabel>
          <Input id="email" name="email" type="email" placeholder="m@example.com" autoComplete="email" {...form.register("email")} />
          <FormMessage className="text-left">{form.formState.errors.email?.message as string}</FormMessage>
        </div>
        <div className="grid gap-3">
          <div className="flex items-center">
            <FormLabel className="text-left" htmlFor="password" invalid={!!form.formState.errors.password}>Password</FormLabel>
            <a href="#" className="ml-auto text-sm underline-offset-4 hover:underline">
              Forgot your password?
            </a>
          </div>
          <Input id="password" name="password" type="password" autoComplete="current-password" {...form.register("password")} />
          <FormMessage className="text-left">{form.formState.errors.password?.message as string}</FormMessage>
        </div>
        {error && <div className="text-red-600 text-sm">{error}</div>}
        <Button type="submit" className="w-full">
          Login
        </Button>
        <div className="after:border-border relative text-center text-sm after:absolute after:inset-0 after:top-1/2 after:z-0 after:flex after:items-center after:border-t">
          <span className="bg-background text-muted-foreground relative z-10 px-2">
            Or continue with
          </span>
        </div>
        <div className="grid grid-cols-2 gap-2">
          <Button variant="outline" className="w-full" type="button" onClick={() => oauth("google")}>
            Google
          </Button>
          <Button variant="outline" className="w-full" type="button" onClick={() => oauth("apple")}>
            Apple
          </Button>
        </div>
      </div>
      <div className="text-center text-sm">
        Don&apos;t have an account?{" "}
        <Link to="/register" className="underline underline-offset-4">
          Sign up
        </Link>
      </div>
    </form>
  );
}
